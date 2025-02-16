using LeadManagementSystem.Comman;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Model.Models;
using Microsoft.Extensions.Configuration;

namespace LeadManagementSystem.Providers.HttpProvider
{
    public class ExternalSMSProvider : BaseHttpProvider
    {
        private readonly IConfiguration _config;
        private readonly string _hostWebApiUrl;

        public ExternalSMSProvider(IConfiguration config)
        {
            _config = config;
            _hostWebApiUrl = config["Service:SMSUrl"];
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


        public SMSRequest CreateSmsRequest(LeadRequest leadRequest, string messageTypeCode, LeadSourceModel leadSource)
        {
            return new SMSRequest
            {
                MessageTypeCode = messageTypeCode,
                AccountName = ApplicationConstant.AccountName,
                PhoneNumber = leadRequest.CellPhone,
                SourceSystemCode = ApplicationConstant.SystemCode,
                SourceSystemReference = leadRequest.Id.ToString(),
                IdNumber = leadRequest.IDNumber,
                MplNumber = 0,
                Language = ApplicationConstant.Language,
                PersonalisationData = new PersonalisationData
                {
                    LeadSourceNumber = leadSource.TelephoneNumber.ToString()
                },
                ContextData = new ContextData
                {
                    ProductID = leadSource.BrandId
                }
            };
        }
    }
}
