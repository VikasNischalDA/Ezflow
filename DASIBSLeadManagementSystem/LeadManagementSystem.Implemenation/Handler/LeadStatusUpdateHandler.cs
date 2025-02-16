using AutoMapper;
using LeadManagementSystem.Comman;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Data;
using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LeadStatusUpdateRequest = LeadManagementSystem.Contracts.Request.LeadStatusUpdateRequest;

namespace LeadManagementSystem.Logic.Handler
{
    public class LeadStatusUpdateHandler : IRequestHandler<LeadStatusUpdateRequest, ActionResult<LeadStatusUpdateResponse>>
    {
        private readonly AppDbContext appDbContext;
        private readonly IMapper mapper;
        private readonly ILogger<LeadStatusUpdateHandler> logger;
        private readonly SmsService smsService;

        public LeadStatusUpdateHandler(AppDbContext _appDbContext,
                                       IMapper _mapper,
                                       ILogger<LeadStatusUpdateHandler> _logger,SmsService _smsService)
        {
            appDbContext = _appDbContext;
            mapper = _mapper;
            logger = _logger;
            smsService = _smsService;
        }

        public async Task<ActionResult<LeadStatusUpdateResponse>> Handle(LeadStatusUpdateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Processing lead status update request: {@Request}", request);
                await UpdateBrandAppIdAndBrandId(request);
                await UpdateLeadStatusAsync(request);
                logger.LogInformation("Lead status update processed successfully");
                return CreateResponse(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing lead status update request");
                return CreateResponse(false, ex.Message);
            }

        }

        private async Task UpdateBrandAppIdAndBrandId(LeadStatusUpdateRequest request)
        {
            bool updateRequired = false;
            var existingLeadResponse = await appDbContext.LeadResponseModel
                   .FirstOrDefaultAsync(x => x.LeadId == request.LeadId) ?? throw new InvalidOperationException($"No Lead with {request.LeadId} found");
            if (existingLeadResponse != null)
            {
                if (existingLeadResponse.BrandApplicationId != request.BrandAppID)
                {
                    existingLeadResponse.BrandApplicationId = request.BrandAppID;
                    updateRequired = true;
                }
                if (existingLeadResponse.BrandId != request.BrandID)
                {
                    existingLeadResponse.BrandId = request.BrandID;
                    updateRequired = true;
                }
            }
            if (updateRequired)
            {
                await appDbContext.SaveChangesAsync();
            }
        }

        private ActionResult<LeadStatusUpdateResponse> CreateResponse(bool success, string errorMessage = null)
        {
            var response = new LeadStatusUpdateResponse
            {
                WebServiceMessage = new WebServiceMessage
                {
                    Success = success,
                    ErrorMessage = errorMessage
                }
            };

            var actionResultCode = success ? ActionResultCode.Success : ActionResultCode.Error;
            return new ActionResult<LeadStatusUpdateResponse>(actionResultCode, response);
        }

        private async Task UpdateLeadStatusAsync(LeadStatusUpdateRequest leadStatusRequest)
        {
            await FetchAndUpdateLeadStatus(leadStatusRequest.LeadId, leadStatusRequest);
            logger.LogInformation($"Lead status for LeadID {leadStatusRequest.LeadId} updated in the database.");
        }

        private async Task<LeadStatusModel> FetchAndUpdateLeadStatus(int leadId, LeadStatusUpdateRequest request)
        {
            try
            {
                var existingLeadStatus = await appDbContext.LeadStatusModel
                                             .AsNoTracking()
                                             .OrderByDescending(x => x.ModifiedDate)
                                             .FirstOrDefaultAsync(x => x.LeadId == leadId) ??
                                         throw new InvalidOperationException($"LeadId {leadId} not found");
                request.SubStatusIdFrom = GetSubStatusName(request.SubStatusIdFrom);
                request.SubStatusIdTo = GetSubStatusName(request.SubStatusIdTo);
                if (!LogChanges(existingLeadStatus, request))
                {
                    return existingLeadStatus;
                }
                if (request.StatusIdTo == 10 ) 
                {
                    await HandleLeadDeclineStatus(leadId);

                }
                LeadStatusModel newLeadStatus = MapNewStatus(existingLeadStatus, request);
                newLeadStatus.UMID = existingLeadStatus.UMID;
                await appDbContext.AddAsync(newLeadStatus);
                await appDbContext.SaveChangesAsync();
                logger.LogInformation($"Lead status for LeadID {request.LeadId} updated in the database.");

                return existingLeadStatus;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, $"LeadId {leadId} not found");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unexpected error processing LeadId {leadId}");
                throw new ApplicationException("An unexpected error occurred while updating the lead status.", ex);
            }
        }
        public async Task HandleLeadDeclineStatus(int leadId)
        {
            try
            {
                LeadModel? leadModel = await appDbContext.LeadModel
                .FirstOrDefaultAsync(x => x.Id == leadId);
                LeadSourceModel? leadSourceModel = await appDbContext.LeadSourceModels
                    .FirstOrDefaultAsync(x => x.UMID == leadModel.UMID);

                if (leadModel != null && leadSourceModel != null)
                {
                    LeadRequest leadRequestDetails = mapper.Map<LeadRequest>(leadModel);

                    await smsService.SendSmsAsync(leadRequestDetails, "DeclineAge", leadSourceModel);
                }
                else
                {
                    logger.LogWarning("LeadModel or LeadSourceModel not found for LeadId: {LeadId}", leadId);
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Unexpected error processing.");
                throw new ApplicationException("An unexpected error occurred while fetching lead", ex);

            } 
        }

        private static string GetSubStatusName(string subStatusId)
        {

            if (string.IsNullOrEmpty(subStatusId))
            {
                return null;
            }

            if (int.TryParse(subStatusId, out int parsedSubStatusId) &&
                Enum.IsDefined(typeof(DalasSubStatus), parsedSubStatusId))
            {

                return ((DalasSubStatus)parsedSubStatusId).ToString();
            }

            return subStatusId;
        }

        private LeadStatusModel MapNewStatus(LeadStatusModel existingLeadStatus, LeadStatusUpdateRequest request)
        {
            var leadStatusUpdate = new LeadStatusModel
            {
                LeadId = request.LeadId,
                CreatedDate = DateTime.Now,
                System = SystemType.Dalas.ToString(),
                DalasUserName = request.DALASUserName,
                StatusIdFrom = Enum.GetName(typeof(DalasStatus), request.StatusIdFrom),
                SubStatusIdFrom = GetSubStatusName(request.SubStatusIdFrom),
                StatusIdTo = Enum.GetName(typeof(DalasStatus), request.StatusIdTo),
                SubStatusIdTo = GetSubStatusName(request.SubStatusIdTo),
                ModifiedDate = DateTime.Now
            };

            return leadStatusUpdate;
        }


        private Boolean LogChanges(LeadStatusModel currentLeadStatusModel, LeadStatusUpdateRequest request)
        {
            var changes = new (string, object, object)[]
            {
                ("StatusIdFrom", currentLeadStatusModel.StatusIdFrom, Enum.GetName(typeof(DalasStatus), request.StatusIdFrom)),
                ("StatusIdTo", currentLeadStatusModel.StatusIdTo, Enum.GetName(typeof(DalasStatus), request.StatusIdTo)),
                ("SubStatusIdFrom", currentLeadStatusModel.SubStatusIdFrom, request.SubStatusIdFrom),
                ("SubStatusIdTo", currentLeadStatusModel.SubStatusIdTo, request.SubStatusIdTo)
            };

            if (!changes.Any(change => !Equals(change.Item2, change.Item3)))
            {
                logger.LogInformation($"No changes in Status and SubStatus for LeadId {currentLeadStatusModel.LeadId}.");

                return false;
            }

            foreach (var (field, oldValue, newValue) in changes)
            {
                if (!Equals(oldValue, newValue))
                {
                    logger.LogInformation($"{field} changed from {oldValue} to {newValue}");
                }
            }

            return true;
        }

    }
}