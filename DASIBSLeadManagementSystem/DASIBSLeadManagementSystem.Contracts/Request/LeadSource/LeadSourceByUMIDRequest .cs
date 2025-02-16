using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;

namespace LeadManagementSystem.Contracts.Request.LeadSource
{
    public class LeadSourceByUMIDRequest : IRequest<ActionResult<LeadSourceResponse>>
    {
        public string UMID { get; set; }
    }
}
