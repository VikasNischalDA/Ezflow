using LeadManagementSystem.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadManagementSystem.MVC.Services.Interfaces
{
    public interface IBrandService
    {
       Task<List<BrandResponse>> GetBrands();
    }
}
