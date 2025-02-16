using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using Microsoft.Extensions.Options;

namespace LeadManagementSystem.MVC.Services
{
    public class DashboardService :IDashboardService
    {
        private readonly IBaseHttpClientService _httpClientFactory;
        private readonly ApiConfig _apiConfig;

        public DashboardService(IBaseHttpClientService httpClientFactory, IOptions<ApiConfig> apiConfig)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfig = apiConfig.Value;
        }

        public async Task<ActionResult<PaginationResponse>> GetLeads()
        {

            var response = await _httpClientFactory.GetAsync<PaginationResponse>("DashBoard/leadRecords");
            return response;
           
        }

        public async Task<ActionResult<PaginationResponse>> GetFilterLeadRecords(string? className, int? pageSize, int? pageNumber)
        {
            
            var queryParams = new Dictionary<string, string>();

            if (pageNumber.HasValue)
            {
                queryParams.Add("pageNumber", pageNumber.Value.ToString());
            }

            if (pageSize.HasValue)
            {
                queryParams.Add("pageSize", pageSize.Value.ToString());
            }

            if (!string.IsNullOrEmpty(className))
            {
                queryParams.Add("classType", className);
            }

            var response = await _httpClientFactory.GetAsync<PaginationResponse>("DashBoard/filter", queryParams);          

            return response;
        }
    }
}
