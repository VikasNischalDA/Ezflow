using Microsoft.Extensions.Configuration;

namespace LeadManagementSystem.Providers
{
    public class ExternaLesProvider : BaseHttpProvider
    {

        private readonly IConfiguration _config;
        private readonly string _hostWebApiUrl;

        public ExternaLesProvider(IConfiguration config)
        {
            _config = config;
            _hostWebApiUrl = config["Service:LESUrl"];
        }


        public override HttpClient GetClient()
        {
            string _baseAddress = _hostWebApiUrl;
            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json;charset=UTF-8");
            return client;
        }
    }
}
