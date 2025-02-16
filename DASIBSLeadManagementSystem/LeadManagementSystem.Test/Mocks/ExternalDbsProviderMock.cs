using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Providers.HttpProvider;
using Microsoft.Extensions.Configuration;
using Moq;
using SoapBody = LeadManagementSystem.Contracts.Response.SoapBody;

namespace LeadManagementSystem.Test.Mocks
{
    public static class ExternalDbsProviderMock
    {
        public static Mock<ExternalDbsProvider> CreateMock(
            string dbsUrl = "http://example.com/soap",
            string username = "SGVsbG8sIFdvcmxkIQ==",
            string password = "SGVsbG8sIFdvcmxkIQ=="
        )
        {
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(config => config["Service:DBSUrl"]).Returns(dbsUrl);
            configurationMock.SetupGet(config => config["DBS:UserName"]).Returns(username);
            configurationMock.SetupGet(config => config["DBS:Password"]).Returns(password);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var mock = new Mock<ExternalDbsProvider>(httpClientFactoryMock.Object, configurationMock.Object);

            mock.Setup(provider => provider.GetClient())
                .Returns(() =>
                {
                    var client = new HttpClient
                    {
                        BaseAddress = new Uri(dbsUrl)
                    };
                    client.DefaultRequestHeaders.Add("Accept", "application/xml;charset=UTF-8");
                    return client;
                });

            // Assuming PostSoapXmlAsync is an existing method you want to mock.
            mock.Setup(provider => provider.PostSoapXmlAsync<DbsServiceCall, DBSResponse>(
                It.IsAny<HttpClient>(),
                It.IsAny<DbsServiceCall>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync((HttpClient client, DbsServiceCall request, string url) =>
            {
                var idNumber = request.Body.RequestRiskGrade.IdNumber;

                return
                    idNumber.EndsWith("1221221221221")
                        ? throw new Exception("Provider Failed to provide the Response"): CreateSuccessResponse(idNumber);


            });

            return mock;
        }


        private static DBSResponse CreateSuccessResponse(string idNumber)
        {
            return new DBSResponse
            {
                DbsResponseBody = new SoapBody
                {
                    RequestRiskGradeResponse = new RequestRiskGradeResponse
                    {
                        ReturnRiskGrade = new ReturnRiskGrade
                        {
                            IDNumber = idNumber,
                            BrandID = 123,
                            RiskCellOrProfile = "High",
                            ApplicationFound = true
                        }
                    }
                }
            };
        }
    }
}
