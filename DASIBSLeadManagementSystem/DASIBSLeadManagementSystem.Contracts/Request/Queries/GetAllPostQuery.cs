using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;

namespace LeadManagementSystem.Contracts.Request
{
    public class GetAllPostQuery : IRequest<ActionResult<BaseAPIResponse>>
    {
        public string? Name { get; set; }
        public string? Class { get; set; }
        public string? UMID { get; set; }
    }
}
