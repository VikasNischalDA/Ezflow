using AutoMapper;
using LeadManagementSystem.Comman;
using LeadManagementSystem.Comman.Helpers;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Data;
using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Providers;
using LeadManagementSystem.Providers.HttpProvider;
using LeadManagementSystem.Providers.Interface;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using LeadRequest = LeadManagementSystem.Contracts.Request.LeadRequest;

namespace LeadManagementSystem.Logic.Handler
{
    public class RegisterLeadHandler : IRequestHandler<LeadRequest, ActionResult<LeadResponse>>
    {
        private readonly SmsService smsService;
        private readonly ExternaLesProvider lesProvider;
        private readonly ExternalDbsProvider externalDbsProvider;
        private readonly ExternalDalasProvider externaldalasProvider;
        private readonly IProvider<DbsRequest, DbsServiceCall> dbsServiceCall;
        private readonly IDalasProvider dalasServiceCall;
        private readonly IMapper mapper;
        private readonly AppDbContext appDbContext;
        private readonly string lesEndpoint;
        private readonly string dbsEndpoint;
        private readonly string dalasEndpoint;

        private readonly ILogger<RegisterLeadHandler> logger;


        public RegisterLeadHandler(ExternaLesProvider _lesProvider,
                                   ExternalDbsProvider _externalDbsProvider,
                                   ExternalDalasProvider _externalDalasProvider,
                                   SmsService _smsService,
                                   IProvider<DbsRequest, DbsServiceCall> _dbsServiceCall,
                                   IDalasProvider _dalasServiceCall,
                                   AppDbContext _appDbContext,
                                   IMapper _mapper,
                                   IConfiguration configuration,
                                   ILogger<RegisterLeadHandler> _logger)
        {
            lesProvider = _lesProvider;
            externalDbsProvider = _externalDbsProvider;
            externaldalasProvider = _externalDalasProvider;
            smsService= _smsService;
            dbsServiceCall = _dbsServiceCall;
            dalasServiceCall = _dalasServiceCall;
            appDbContext = _appDbContext;
            mapper = _mapper;
            lesEndpoint = configuration["Service:LesUrl"];
            dbsEndpoint = configuration["Service:DbsUrl"];
            dalasEndpoint = configuration["Service:TurboUrl"];
            logger = _logger;
        }

        public async Task<ActionResult<LeadResponse>> Handle(LeadRequest request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation($"Initiating registration handler for lead request with UMID: {request.UMID}");

                await SaveLeadRequest(request);

                var response = await RegisterLeadInDalas(request);

                Shared.Infrastructure.ActionResult<LeadResponse> Leadresponse = new(ActionResultCode.Success, response);



                return Leadresponse; // based on the logic by calling above method decide what valid

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while processing the lead request for UMID: {request.UMID}");


                var errorResponse = new LeadResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };

                Shared.Infrastructure.ActionResult<LeadResponse> leadErrorResponse = new(ActionResultCode.Error, errorResponse);

                return leadErrorResponse;
            }
        }

        private async Task<LeadResponse> RegisterLeadInDalas(LeadRequest request)
        {

            logger.LogInformation("Initiating lead registration in DALAS.");

            DBSResponse dBSResponse = null;

            var leadSourceDetails = await GetBrandIdAndLeadSource(request);

            logger.LogInformation($"Lead source details retrieved. UMID: {leadSourceDetails.UMID}, Brand: {leadSourceDetails.Brand}, LeadSource: {leadSourceDetails.LeadSourceClass}");

            var lesResponse = await GetLesDecision(request, leadSourceDetails);

            if (lesResponse.Decision.Equals(ApplicationConstant.DecisionApprove) || lesResponse.Decision.Equals(ApplicationConstant.DecisionMayBe))
            {
                await SaveLeadStatus(request, null, DalasStatus.OutBoundLead.ToString(), lesResponse.Decision == "Approve" ? DalasStatus.LESApproved.ToString() : DalasStatus.LESMayBe.ToString());

                dBSResponse = await GetDBSDecision(request, leadSourceDetails, lesResponse);
            }

            if (lesResponse.Decision.Equals(ApplicationConstant.DecisionDecline))
            {
                await SaveLeadStatus(request, null, DalasStatus.OutBoundLead.ToString(), DalasStatus.LESDeclined.ToString());
       
                var leadResponse = new LeadResponse
                {
                    Success = true,
                    Status = lesResponse.Decision,
                    ErrorMessage = String.Join(",", lesResponse.DecisionReasons),
                    LeadId = request.Id.ToString(),
                    BrandId = leadSourceDetails.BrandId.ToString(),
                    Decision = ApplicationConstant.DecisionDecline,
                    LesResponse = JsonConvert.SerializeObject(lesResponse),
                    CreatedDate = DateTime.Now,
                    UMID = request.UMID
                };

                await SaveLeadResponse(leadResponse);

                string? smsType = lesResponse.DecisionReasons.Contains("Customer does not meet company age requirement")
                  ? "DeclineAge"
                  : lesResponse.DecisionReasons.Contains("ConsentData not collected")
                  ? "NoContact.lead"
                  : "Cancel.lead";
                await smsService.SendSmsAsync
                    (request, smsType, leadSourceDetails);

                return leadResponse;
            }

            var dalasResponse = await CreateTurboApplicationOnDalas(request, lesResponse, dBSResponse, leadSourceDetails);

            return dalasResponse;

        }

        private async Task<LeadSourceModel> GetBrandIdAndLeadSource(LeadRequest request)
        {
            try
            {
                var result = await appDbContext.LeadSourceModels.
                                        Include(x => x.BrandModel).
                                        FirstOrDefaultAsync(x => x.UMID == request.UMID);

                if (result == null)
                {
                    logger.LogInformation($"LeadRequest with UMID {request.UMID} not found");
                    throw new Exception($"LeadRequest with the given {request.UMID} not found.");
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to fetch LeadSourceModel for UMID {request.UMID}.");
                throw new Exception("Failed to fetch the umid from lead source model");
            }
        }

        private async Task<LeadResponse> CreateTurboApplicationOnDalas(LeadRequest leadRequest, LesResponse lesResponse, DBSResponse? dBSResponse, LeadSourceModel leadSourceDetails)
        {
            try
            {
                string decision = ApplicationConstant.DecisionApprove;

                var client = externaldalasProvider.GetClient();
                logger.LogInformation("DALAS Call : HTTP client for DALAS service created.");

                var dalasEnvelope = dalasServiceCall.GetSoapEnvlope(leadRequest, lesResponse, leadSourceDetails.BrandId);
                logger.LogInformation("DALAS Call : Created SOAP envelope for DALAS.");

                var dalasResponse = await externaldalasProvider.PostSoapXmlAsync<TurboServiceCall, TurboApplicationContractResponse>(client, dalasEnvelope, dalasEndpoint);

                logger.LogInformation("DALAS Call: Response received from DALAS endpoint: {@DalasResponse}", dalasResponse);


                if (dalasResponse != null && (dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.ApplicationId != null &&
                    dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.BrandApplicationId != null))
                {
                    var customerStatus = await GetCustomerStatus(leadRequest, leadSourceDetails.BrandId);

                    if (dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.Message == ApplicationConstant.ConfirmDeclined
                        || dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.Message == ApplicationConstant.ConsentDeclined ||
                        (dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.Message == ApplicationConstant.TwoMonths && dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.SystemDecision.IsNullOrEmpty()))
                    {
                        var smsType = (dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.Message == ApplicationConstant.ConfirmDeclined ||
                           dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.Message == ApplicationConstant.ConsentDeclined) ? "Cancel.lead" : "DeclineAge";
                        
                        await smsService.SendSmsAsync(leadRequest, smsType, leadSourceDetails);
                        
                        decision = ApplicationConstant.DecisionDecline;
                    }

                    var leadResponse = new LeadResponse
                    {
                        Success = true,
                        Status = customerStatus,
                        ErrorMessage = dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.Message,
                        ApplicationId = dalasResponse?.Body?.TurboApplicationContractResponse?.TurboApplicationContractResult?.ApplicationId?.ToString(),
                        BrandApplicationId = dalasResponse?.Body?.TurboApplicationContractResponse?.TurboApplicationContractResult?.BrandApplicationId?.ToString(),
                        BrandId = leadSourceDetails.BrandId.ToString(),
                        LeadId = leadRequest.Id.ToString(),
                        Decision = decision == ApplicationConstant.DecisionDecline ? "Decline" : "Continue",
                        LesResponse = JsonConvert.SerializeObject(lesResponse).ToString(),
                        DbsResponse = XmlJsonConverter.SerializeToXml(dBSResponse).ToString(),
                        TurboResponse = XmlJsonConverter.SerializeToXml(dalasResponse).ToString(),
                        CreatedDate = DateTime.Now,
                        UMID = leadRequest.UMID
                    };


                    await SaveLeadResponse(leadResponse);

                    await SaveLeadStatus(leadRequest, dalasResponse, lesResponse.Decision == "Approve" ? DalasStatus.LESApproved.ToString() : DalasStatus.LESMayBe.ToString(), DalasStatus.ApplicationCreated.ToString());

                    logger.LogInformation("DALAS Call: Lead successfully registered with DALAS. Lead ID: {LeadId}", leadRequest.Id);
                    logger.LogInformation("Lead Response: {@LeadResponse}", leadResponse);
                    return leadResponse;

                }

                var response = new LeadResponse
                {
                    Success = true,
                    Status = dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.ResultCode,
                    LeadId = leadRequest.Id.ToString(),
                    BrandId = leadSourceDetails.BrandId.ToString(),
                    Decision = ApplicationConstant.DecisionDecline,
                    ErrorMessage = dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.Message,
                    LesResponse = JsonConvert.SerializeObject(lesResponse).ToString(),
                    DbsResponse = XmlJsonConverter.SerializeToXml(dBSResponse).ToString(),
                    TurboResponse = XmlJsonConverter.SerializeToXml(dalasResponse).ToString(),
                    CreatedDate = DateTime.Now,
                    UMID = leadRequest.UMID
                };

                await SaveLeadStatus(leadRequest, dalasResponse, lesResponse.Decision == "Approve" ? DalasStatus.LESApproved.ToString() : DalasStatus.LESMayBe.ToString(), dalasResponse.Body.TurboApplicationContractResponse.TurboApplicationContractResult.ResultCode);

                logger.LogInformation("DALAS Call : Retrieve a empty response from the DALAS endpoint.");
                await SaveLeadResponse(response);

                return response;
            }
            catch (Exception ex)
            {
                var leadResponse = new LeadResponse()
                {
                    Success = false,
                    LeadId = leadRequest.Id.ToString(),
                    BrandId = leadSourceDetails.BrandId.ToString(),
                    Decision = ApplicationConstant.DecisionDecline,
                    LesResponse = JsonConvert.SerializeObject(lesResponse).ToString(),
                    DbsResponse = JsonConvert.SerializeObject(dBSResponse).ToString(),
                    TurboResponse = ex.Message,
                    ErrorMessage = "Failed to create turbo application on dalas",
                    CreatedDate = DateTime.Now,
                    UMID = leadRequest.UMID
                };

                await SaveLeadResponse(leadResponse);

                logger.LogError(ex, "DALAS Call : Failed to create turbo application on DALAS endpoint.");
                throw new Exception("Failed to create turbo application on dalas");
            }
        }


        private async Task<DBSResponse> GetDBSDecision(LeadRequest request, LeadSourceModel leadSourceModel, LesResponse? lesResponse)
        {
            try
            {
                if (leadSourceModel.BrandModel != null)
                {
                    var dbsRequest = new DbsRequest()
                    {
                        BrandId = leadSourceModel.BrandModel.BrandId,
                        IdNumber = request.IDNumber
                    };

                    var client = externalDbsProvider.GetClient();
                    logger.LogInformation("DBS Call: HTTP client for DBS service created.");

                    var soapEnvelope = dbsServiceCall.GetSoapEnvlope(dbsRequest);
                    logger.LogInformation("DBS Call: SOAP envelope created for DBS request.");


                    var dbsResponse = await externalDbsProvider.PostSoapXmlAsync<DbsServiceCall, DBSResponse>(client, soapEnvelope, dbsEndpoint);
                    logger.LogInformation("DBS Call: Response received from DBS endpoint: {@DbsResponse}", dbsResponse);

                    return dbsResponse;
                }
                else
                {
                    var leadResponse = new LeadResponse()
                    {
                        Success = false,
                        LeadId = request.Id.ToString(),
                        BrandId = leadSourceModel.BrandId.ToString(),
                        Decision = ApplicationConstant.DecisionDecline,
                        LesResponse = JsonConvert.SerializeObject(lesResponse).ToString(),
                        ErrorMessage = "BrandId is not found for the given" + request.UMID,
                        CreatedDate = DateTime.Now,
                        UMID = request.UMID
                    };

                    await SaveLeadResponse(leadResponse);

                    logger.LogError("DBS Call: Brand ID not found for UMID {UMID}", request.UMID);
                    throw new Exception($"BrandId is not found for the given {request.UMID}");
                }
            }
            catch (Exception ex)
            {
                var leadResponse = new LeadResponse()
                {
                    Success = false,
                    LeadId = request.Id.ToString(),
                    BrandId = leadSourceModel.BrandId.ToString(),
                    Decision = ApplicationConstant.DecisionDecline,
                    LesResponse = JsonConvert.SerializeObject(lesResponse).ToString(),
                    DbsResponse = ex.Message,
                    ErrorMessage = "Failed to get the response from dbs endpoint",
                    CreatedDate = DateTime.Now,
                    UMID = request.UMID
                };

                await SaveLeadResponse(leadResponse);

                logger.LogError(ex, $"DBS Call: Failed to get response from DBS endpoint.");
                throw new Exception("Failed to get the response from dbs endpoint");
            }
        }

        private async Task<LesResponse> GetLesDecision(LeadRequest request, LeadSourceModel leadSourceDetails)
        {
            try
            {
                var lesRequest = new LesRequest()
                {
                    BrandName = leadSourceDetails.Brand,
                    IdNumber = request.IDNumber,
                    LeadSource = leadSourceDetails.LeadSourceClass,
                    Source = ApplicationConstant.Source,
                    UMID = request.UMID
                };

                var client = lesProvider.GetClient();
                logger.LogInformation("LES Call:HTTP client for LES service created.");

                var lesResponse = await lesProvider.PostAsync<LesRequest, LesResponse>(client, lesRequest, lesEndpoint);
                logger.LogInformation("LES Call: Response received from LES Endpoint: {@LesResponse}", lesResponse);

                return lesResponse;
            }
            catch (Exception ex)
            {
                var leadResponse = new LeadResponse()
                {
                    Success = false,
                    LeadId = request.Id.ToString(),
                    BrandId = leadSourceDetails.BrandId.ToString(),
                    Decision = ApplicationConstant.DecisionDecline,
                    LesResponse = ex.Message,
                    ErrorMessage = "Failed to get the response from les endpoint",
                    CreatedDate = DateTime.Now,
                    UMID = request.UMID
                };

                await SaveLeadResponse(leadResponse);

                logger.LogError(ex, $"LES Call: Failed to get LES response.");
                throw new Exception("Failed to get the response from les endpoint");
            }

        }

        private async Task<string> GetCustomerStatus(LeadRequest leadRequest, int brandId)
        {
            try
            {
                var client = externaldalasProvider.GetClient();
                logger.LogInformation("DALAS Call : HTTP client for GetCustomerStatus service created.");

                var customerStatusEnvelop = dalasServiceCall.GetSoapEnvlope(leadRequest, brandId);
                logger.LogInformation("DALAS Call: Created SOAP envelope for GetCustomerStatus.");

                var customerStatusResponse = await externaldalasProvider.PostSoapXmlAsync<CustomerStatusRequest, CustomerStatusResponse>(client, customerStatusEnvelop, dalasEndpoint);
                logger.LogInformation("DALAS Call: Response received from GetCustomerStatus endpoint: {@CustomerStatusResponse}", customerStatusResponse);

                await SaveGetCustomerStatusResponse(customerStatusResponse, leadRequest.Id);

                var applicationStatus = customerStatusResponse.Body.GetCustomerStatusResponse.GetCustomerStatusResult.CustomerStatus.CustomerBrandStatus.ApplicationStatus;

                if (applicationStatus == null)
                {
                    logger.LogError($"Retrieve empty application status");
                    throw new Exception("Application not found");
                }

                return applicationStatus;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DALAS Call: Failed to get customer status response for request {@LeadRequest}.", leadRequest);
                throw new Exception("Failed to get the response from get customer status endpoint");
            }
        }

        #region SaveMethods
        private async Task SaveLeadRequest(LeadRequest request)
        {
            try
            {
                var leadModel = mapper.Map<LeadModel>(request);
                appDbContext.LeadModel.Add(leadModel);
                await appDbContext.SaveChangesAsync();
                request.Id = leadModel.Id;
                logger.LogInformation("Lead request saved successfully. Request details: {@LeadRequest}", request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to save lead request: {@LeadRequest}", request);
                throw;
            }
        }

        private async Task SaveLeadResponse(LeadResponse leadResponse)
        {
            try
            {
                var leadResponseModel = mapper.Map<LeadResponseModel>(leadResponse);
                appDbContext.LeadResponseModel.Add(leadResponseModel);
                await appDbContext.SaveChangesAsync();
                leadResponse.Id = leadResponseModel.Id;
                logger.LogInformation($"Lead response saved successfully. Response ID: {leadResponse.Id}");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to save lead response.");
                throw new Exception("Failed to save the lead response");
            }
        }

        private async Task SaveGetCustomerStatusResponse(CustomerStatusResponse customerStatusResponse, int leadId)
        {
            try
            {
                var getCustomerStatusResponseModel = mapper.Map<GetCustomerStatusResponseModel>(customerStatusResponse.Body.GetCustomerStatusResponse.GetCustomerStatusResult);
                getCustomerStatusResponseModel.LeadId = leadId;
                appDbContext.GetCustomerStatusResponseModel.Add(getCustomerStatusResponseModel);
                await appDbContext.SaveChangesAsync();
                logger.LogInformation($"Customer status response saved successfully. Response ID: {getCustomerStatusResponseModel.Id} ");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to save customer status response.");
                throw new Exception("Failed to save the getcustomerstatus response.");
            }
        }

        private async Task SaveLeadStatus(LeadRequest request, TurboApplicationContractResponse? dalasResponse, string status, string subStatus)
        {
            try
            {
                var leadSourceDetails = await GetBrandIdAndLeadSource(request);
                var leadId = request.Id;


                var leadStatusUpdate = new LeadStatusModel
                {
                    LeadId = leadId,
                    CreatedDate = DateTime.Now,
                    DalasUserName = string.Empty,
                    StatusIdFrom = status,
                    StatusIdTo = subStatus,
                    SubStatusIdFrom = null,
                    SubStatusIdTo = null,
                    System = ApplicationConstant.Source,
                    UMID = request.UMID
                };


                appDbContext.Entry(leadStatusUpdate).State = EntityState.Added;
                await appDbContext.LeadStatusModel.AddAsync(leadStatusUpdate);
                await appDbContext.SaveChangesAsync();
                appDbContext.Entry(leadStatusUpdate).State = EntityState.Detached;
                logger.LogInformation($"Lead status saved successfully. Lead ID: {leadId}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to save lead status for leadId: {request.Id}");
                throw;
            }

        }

        #endregion
    }
}
