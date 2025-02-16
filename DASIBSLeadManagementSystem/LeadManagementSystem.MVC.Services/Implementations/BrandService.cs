using LeadManagementSystem.MVC.Services.Interfaces;
using LeadManagementSystem.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.MVC.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBaseHttpClientService _httpClientFactory;
        public BrandService(IBaseHttpClientService httpClientFactory)
        {
          _httpClientFactory = httpClientFactory;
        }
        public async Task<List<BrandResponse>> GetBrands()
        {            
            var result = await _httpClientFactory.GetAsync<List<BrandResponse>>("Brand/Brand-List");
            return result.Entity.ToList();
        }
    }
}
