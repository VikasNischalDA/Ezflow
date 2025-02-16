using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Providers.Interface;
using Microsoft.Extensions.Configuration;

namespace LeadManagementSystem.Providers.HttpProvider
{
    public class ExternalDbsProvider : BaseHttpProvider, IProvider<DbsRequest, DbsServiceCall>
    {
        private readonly string _soapEndpoint;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _username;
        private readonly string _password;

        public ExternalDbsProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _soapEndpoint = configuration["Service:DBSUrl"];
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

        public DbsServiceCall GetSoapEnvlope(DbsRequest dbsRequest)
        {
            return new DbsServiceCall
            {
                Header = new SoapHeader
                {
                    Authentication = new Authentication
                    {
                        Username = DecodeBase64(_username),
                        Password = DecodeBase64(_password)
                    }
                },
                Body = new SoapBody
                {
                    RequestRiskGrade = dbsRequest
                }
            };
        }
    }
}
