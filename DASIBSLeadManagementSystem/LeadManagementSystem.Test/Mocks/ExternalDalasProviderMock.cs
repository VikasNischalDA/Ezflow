using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Providers.HttpProvider;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LeadManagementSystem.Test.Mocks
{
    public static class ExternalDalasProviderMock
    {
        public const string ConfirmDeclined = "Cancelled: Message To be confirmed";
        public const string ConsentDeclined = "Cancelled: Consent Declined";
        private const string TwoMonthsMessage = "Thank you for applying for a personal loan. Unfortunately your application was unsuccessful as you do not meet our minimum requirements. You are welcome to re-apply in 2 months.";

        public static Mock<ExternalDalasProvider> CreateMock(string turboUrl = "http://example.com/soap", string username = "encodedUsername", string password = "encodedPassword")
        {
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(config => config["Service:TurboUrl"]).Returns(turboUrl);
            configurationMock.SetupGet(config => config["DBS:UserName"]).Returns(username);
            configurationMock.SetupGet(config => config["DBS:Password"]).Returns(password);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var mock = new Mock<ExternalDalasProvider>(httpClientFactoryMock.Object, configurationMock.Object);

            mock.Setup(provider => provider.GetClient())
                .Returns(() =>
                {
                    var client = new HttpClient
                    {
                        BaseAddress = new Uri(turboUrl)
                    };
                    client.DefaultRequestHeaders.Add("Accept", "application/xml;charset=UTF-8");
                    return client;
                });

            mock.Setup(provider => provider.PostSoapXmlAsync<TurboServiceCall, TurboApplicationContractResponse>(
                It.IsAny<HttpClient>(),
                It.IsAny<TurboServiceCall>(),
                It.IsAny<string>()))
                .ReturnsAsync((HttpClient client, TurboServiceCall request, string url) => HandleTurboServiceCall(request));

            mock.Setup(provider => provider.PostSoapXmlAsync<CustomerStatusRequest, CustomerStatusResponse>(
                It.IsAny<HttpClient>(),
                It.IsAny<CustomerStatusRequest>(),
                It.IsAny<string>()))
                .ReturnsAsync((HttpClient client, CustomerStatusRequest request, string url) => HandleCustomerStatusRequest(request));

            return mock;
        }

        private static TurboApplicationContractResponse HandleTurboServiceCall(TurboServiceCall request)
        {
            var Surname = request.Body.TurboApplicationContract.TurboInput.PersonalInfo.Person.PersonSurname;

            return Surname switch
            {
               
                _ when Surname.EndsWith("INVALID") => throw new Exception("Provider Failed to provide the Response"),
                _ when Surname.EndsWith("cancel") => CreateTurboCancelledResponse(),
                _ when Surname.EndsWith("twomonths") => CreateTurboTwoMonthsResponse(),
                _ when Surname.EndsWith("empty") => null,
                _ => CreateTurboDeclineResponse()
            };
        }

        private static CustomerStatusResponse HandleCustomerStatusRequest(CustomerStatusRequest request)
        {
            var idNumber = request.CustomerStatusBody.GetCustomerStatusRequest.CustomerStatusInput.IdNumber;

            if (idNumber == "1111111111111")
            {
                throw new Exception("Provider Failed to provide the Response");
            }

            if (idNumber == "0000000000000")
            {
                return null;
            }

            CustomerBrandStatus CreateCustomerBrandStatus(string applicationStatus, string applicationSubStatus) => new CustomerBrandStatus
            {
                BrandId = 1,
                IsRepeat = true,
                HasApplication = false,
                ReApplyRestrictionDate = DateTime.Now.AddMonths(1),
                ApplicationStatus = applicationStatus,
                ApplicationSubStatus = applicationSubStatus,
                IsRepeatExpired = false
            };

            if (idNumber == "2222222233333")
            {
                return new CustomerStatusResponse
                {
                    Body = new CustomerStatusResponseBody
                    {
                        GetCustomerStatusResponse = new GetCustomerStatusResponse
                        {
                            GetCustomerStatusResult = new GetCustomerStatusResult
                            {


                                CustomerStatus = new CustomerStatus
                                {
                                    CustomerBrandStatus = new CustomerBrandStatus
                                    {
                                        ApplicationStatus = "somestatus",


                                    }
                                }
                            }
                        }
                    }
                };
            }
            var isError = idNumber == "1212122121212";
            var result = isError ? "Error" : "Success";
            var messageCode = isError ? "500" : "200";
            var message = isError ? "Error processing request" : "Request processed successfully";
            var responseIdNumber = isError ? "1111212111212" : idNumber;
            var applicationStatus = isError ? null : "Approved";

           
            return new CustomerStatusResponse
            {
                Body = new CustomerStatusResponseBody
                {
                    GetCustomerStatusResponse = new GetCustomerStatusResponse
                    {
                        GetCustomerStatusResult = new GetCustomerStatusResult
                        {
                            Result = result,
                            MessageCode = messageCode,
                            Message = message,
                            IdNumber = responseIdNumber,
                            CustomerStatus = new CustomerStatus
                            {
                                CustomerBrandStatus = CreateCustomerBrandStatus(applicationStatus, "Pending Review")
                            }
                        }
                    }
                }
            };
        }

     
        private static TurboApplicationContractResponse CreateTurboCancelledResponse() =>
            new()
            {
                Body = new TurboResponseSoapBody
                {
                    TurboApplicationContractResponse = new TurboApplicationContractResponseBody
                    {
                        TurboApplicationContractResult = new TurboApplicationContractResult
                        {
                            InternetTurboApplicationId = 0,
                            SystemDecision = "Cancelled",
                            Message = ConfirmDeclined,
                            LeadId = 134,
                           
                          
                            ResultCode = "02",
                            RiskLevel = 0,
                        }
                    }
                }
            };

        private static TurboApplicationContractResponse CreateTurboTwoMonthsResponse() =>
            new()
            {
                Body = new TurboResponseSoapBody
                {
                    TurboApplicationContractResponse = new TurboApplicationContractResponseBody
                    {
                        TurboApplicationContractResult = new TurboApplicationContractResult
                        {
                            InternetTurboApplicationId = 0,
                            SystemDecision = "",
                            Message = TwoMonthsMessage,
                            LeadId = 129,
                          
                            ResultCode = "01",
                            RiskLevel = 0,
                        }
                    }
                }
                };

        private static TurboApplicationContractResponse CreateTurboDeclineResponse() =>
            new()
            {
                Body = new TurboResponseSoapBody
                {
                    TurboApplicationContractResponse = new TurboApplicationContractResponseBody
                    {
                        TurboApplicationContractResult = new TurboApplicationContractResult
                        {

                            InternetTurboApplicationId = 12345,
                            SystemDecision = "Decline",
                            Message = ConsentDeclined,
                            LeadId = 133,
                            ApplicationId = "123",
                            BrandApplicationId = 456,
                            ResultCode = "00",
                            RiskLevel = 1,
                        }
                    }
                }
            };

       
    }
}
