using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Shared.Contracts.Request;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace LeadManagementSystem.MVC.Services
{
    public class AuthenticationService :IAuthenticationService
    {
        private readonly IBaseHttpClientService _httpClientFactory;
        private readonly ApiConfig _apiConfig;

        public AuthenticationService(IBaseHttpClientService httpClientFactory, IOptions<ApiConfig> apiConfig)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfig = apiConfig.Value;
        }

        public async Task<ActionResult<LoginResponse>> AuthenticateAsync(LoginRequest loginDetails)
        {
            return await _httpClientFactory.PostAsync<LoginRequest, LoginResponse>(loginDetails, "Authenticate");
        }
    }
}
