using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Shared.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.MVC.Services
{
    public class BaseHttpClientService : IBaseHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiConfig _apiConfig;

        public BaseHttpClientService(IHttpClientFactory httpClientFactory, IOptions<ApiConfig> apiConfig)
        {
            _httpClientFactory = httpClientFactory;
            _apiConfig = apiConfig.Value;
        }
        public async Task<ActionResult<TResponse>> GetAsync<TResponse>(string endpoint, Dictionary<string, string> queryParams = null)
        {
            var client = _httpClientFactory.CreateClient(_apiConfig.ApiHttpClient);

            var uriBuilder = new UriBuilder(client.BaseAddress + endpoint);
            if (queryParams != null && queryParams.Any())
            {
                var query = new StringBuilder();
                foreach (var param in queryParams)
                {
                    if (query.Length > 0)
                        query.Append("&");
                    query.Append($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}");
                }
                uriBuilder.Query = query.ToString();
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            var response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ActionResult<TResponse>>(responseContent);
                return result;
            }
            else
            {
               // var errorContent = await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        public async Task<ActionResult<TResponse>> PostAsync<TRequest, TResponse>(TRequest request, string endpoint)
        {
            var client = _httpClientFactory.CreateClient(_apiConfig.ApiHttpClient);

            var requestJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = content
            };

            var response = await client.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ActionResult<TResponse>>(responseContent);
                return result;
            }
            //else
            //{
            //    //var errorContent = await response.Content.ReadAsStringAsync();
            //}
            return null;
        }

        
    }
}
