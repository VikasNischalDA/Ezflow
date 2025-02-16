using LeadManagementSystem.Providers;
using Moq;
using Microsoft.Extensions.Configuration;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Contracts.Request;

namespace LeadManagementSystem.Test.Mocks
{
    public static class ExternaLesProviderMock
    {
        public static Mock<ExternaLesProvider> CreateMock(string lesUrl = "http://dauapp35/LeadEvaluationService/api/v2/leadevaluation")
        {
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(config => config["Service:LESUrl"]).Returns(lesUrl);
            var mock = new Mock<ExternaLesProvider>(configurationMock.Object);

            mock.Setup(x => x.PostAsync<LesRequest, LesResponse>(It.IsAny<HttpClient>(), It.IsAny<LesRequest>(), It.IsAny<string>()))
                .ReturnsAsync((HttpClient client, LesRequest request, string UMID) =>


                {
                    if (request.IdNumber == "1212131314141")
                    {
                        return new LesResponse
                        {
                            Decision = "Decline",
                            BrandName = "Sample Brand",
                            DecisionReasons = new List<string> { "Reason 1", "Reason 2" },
                            CustomerType = "Individual",
                            DoNotPromote = new DoNotPromote
                            {
                                AllowEmail = true,
                                AllowMail = false,
                                AllowSms = true,
                                AllowTelephone = false
                            },
                            DecisionCodes = new List<string> { "Code1", "Code2" },
                            MessageId = "Message123"
                        };
                    }
                    else if (request.IdNumber.EndsWith("4545454512121"))
                    {
                        throw new Exception("Provider failed to provide response");
                    }

                    else if (request.IdNumber == "8888889999999")
                    {
                        return new LesResponse
                        {
                            Decision = "Approve",
                            BrandName = "Another Brand",
                            DecisionReasons = new List<string> { "Reason A", "Reason B" },
                            CustomerType = "Corporate",
                            DoNotPromote = new DoNotPromote
                            {
                                AllowEmail = false,
                                AllowMail = true,
                                AllowSms = false,
                                AllowTelephone = true
                            },
                            DecisionCodes = new List<string> { "CodeX", "CodeY" },
                            MessageId = "Message456"
                        };
                    }

                    else
                    {
                        return new LesResponse { Decision = "Maybe" };
                    }
                });

            return mock;
        }
    }
}
