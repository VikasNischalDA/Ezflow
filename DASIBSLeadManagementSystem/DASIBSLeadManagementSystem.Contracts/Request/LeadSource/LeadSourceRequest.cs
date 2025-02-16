using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Contracts.Request;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;

namespace LeadManagementSystem.Contracts.Request.LeadSource
{
    public class LeadSourceRequest : LeadSourceContract, IRequest<ActionResult<bool>>
    {
        
    }

    public class LeadSourceVM
    {
        public List<BrandResponse>? BrandList { get; set; } = new List<BrandResponse>();
    }
}
