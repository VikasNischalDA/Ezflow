using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;

namespace LeadManagementSystem.Contracts.Request
{
    public class LeadRecordRequest : Shared.Contracts.Request.LeadRecordRequest, IRequest<ActionResult<PaginationResponse>>
    {
        public string? ClassType { get; set; }
    }
}
