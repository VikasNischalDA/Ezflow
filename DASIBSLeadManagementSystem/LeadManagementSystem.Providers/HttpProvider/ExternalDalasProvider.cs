using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Providers.Interface;
using Microsoft.Extensions.Configuration;
using LeadRequest = LeadManagementSystem.Contracts.Request.LeadRequest;

namespace LeadManagementSystem.Providers.HttpProvider
{
    public class ExternalDalasProvider : BaseHttpProvider, IDalasProvider
    {
        private readonly string _soapEndpoint;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _username;
        private readonly string _password;
        private static string _turbo = "Turbo";
        private static string _ezFlow = "EzFlow";

        public ExternalDalasProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _soapEndpoint = configuration["Service:TurboUrl"];
            _username = configuration["DBS:UserName"];
            _password = configuration["DBS:Password"];
        }

        public override HttpClient GetClient()
        {
            string _baseAddress = _soapEndpoint;
            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };
            client.DefaultRequestHeaders.Add("Accept", "application/xml;charset=UTF-8");
            return client;
        }

        public TurboServiceCall GetSoapEnvlope(LeadRequest leadRequest, LesResponse lesResponse, int brandId)
        {
            if(lesResponse == null)
            {
                throw new Exception("LES response cannot be null");
            }

            return new TurboServiceCall
            {
                Body = new TurboSoapBody
                {
                    TurboApplicationContract = new TurboApplicationContractRequest
                    {
                        TurboInput = new TurboInput
                        {
                            ProductId = brandId,
                            PromotionalCode = leadRequest.UMID,
                            ApplicationType = "Turbo",
                            ApplicationSource = "Ezflow",
                            LeadId = leadRequest.Id.ToString(),
                            LeadEvaluationMessageId = lesResponse.MessageId,
                            PersonalInfo = new PersonalInfo
                            {
                                Person = new Person
                                {
                                    PersonName = leadRequest.FirstName,
                                    PersonSurname = leadRequest.Surname,
                                    PersonIDNumber = leadRequest.IDNumber
                                },
                                HomeTelephoneNumber = leadRequest.HomeNumber != null ? new TelephoneNumber
                                {
                                    Code = leadRequest.HomeNumber.Substring(0,3),
                                    Number = leadRequest.HomeNumber.Substring(3)
                                } : null,
                                WorkTelephoneNumber = leadRequest.WorkNumber != null ? new TelephoneNumber
                                {
                                    Code =  leadRequest.WorkNumber.Substring(0,3),
                                    Number = leadRequest.WorkNumber.Substring(3)
                                } : null,
                                CellNumber = new TelephoneNumber
                                {
                                    Code = leadRequest.CellPhone.Substring(0,3),
                                    Number = leadRequest.CellPhone.Substring(3)
                                },
                                EmailAddress = leadRequest.Email ?? null,
                                GrossIncome = leadRequest.GrossIncome,
                                AllowsCreditCheck = leadRequest.AllowsCreditCheck.ToString() == "true" ? "Y" : "N",
                            }
                        }
                    }
                }
            };
        }

        public CustomerStatusRequest GetSoapEnvlope(LeadRequest request, int brandId)
        {
            return new CustomerStatusRequest
            {
                CustomerStatusBody = new CustomerStatusBody
                {
                    GetCustomerStatusRequest = new GetCustomerStatusRequest
                    {
                        CustomerStatusInput = new CustomerStatusInput
                        {
                            IdNumber = request.IDNumber,
                            BrandIds = new List<int> { brandId }
                        }
                    }
                }
            };
        }
    }
 }
