using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using Microsoft.Extensions.Options;

namespace LeadManagementSystem.MVC.Services
{
    public class LeadSourceService : ILeadSourceService
    {
        private readonly IBaseHttpClientService _httpClientFactory;
        private readonly ApiConfig _apiConfig;

        public LeadSourceService(IBaseHttpClientService httpClientFactory, IOptions<ApiConfig> apiConfig)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfig = apiConfig.Value;
        }

        public async Task<ActionResult<BaseAPIResponse>> GetFilterLeadSource(string name, string umid, string technicalClass)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(name))
            {
                queryParams.Add("Name", name);
            }

            if (!string.IsNullOrEmpty(umid))
            {
                queryParams.Add("UMID", umid);
            }

            if (!string.IsNullOrEmpty(technicalClass))
            {
                queryParams.Add("Class", technicalClass);
            }

            var response = await _httpClientFactory.GetAsync<BaseAPIResponse>("LeadSource/GetLeadSourceDetails", queryParams);

            return response;
        }

        public async Task<ActionResult<BaseAPIResponse>> GetLeadSource()
        {
            var response = await _httpClientFactory.GetAsync<BaseAPIResponse>("LeadSource/GetLeadSourceDetails");
            return response;
        }

        public async Task<ActionResult<LeadSourceResponse>> GetLeadSourceByUMID(string UMID)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(UMID))
            {
                queryParams.Add("UMID", UMID);
            }
            var response = await _httpClientFactory.GetAsync<LeadSourceResponse>("LeadSource/GetByUMID", queryParams);

            return response;
        }

        public async Task<ActionResult<bool>> UpdateLeadSource(LeadSourceRequest model)
        {
            var response = await _httpClientFactory.PostAsync<LeadSourceRequest, bool>(model, "LeadSource/SaveLeadSource");

            return response;
        }
    }
}
