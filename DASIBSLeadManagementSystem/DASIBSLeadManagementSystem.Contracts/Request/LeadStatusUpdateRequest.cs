
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;

namespace LeadManagementSystem.Contracts.Request
{
    public class LeadStatusUpdateRequest : LeadManagementSystem.Shared.Contracts.Request.LeadStatusUpdateRequest, IRequest<ActionResult<LeadStatusUpdateResponse>>
    {
    }
}
