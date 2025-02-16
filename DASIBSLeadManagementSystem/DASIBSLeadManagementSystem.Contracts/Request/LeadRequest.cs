using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;


namespace LeadManagementSystem.Contracts.Request
{

    public class LeadRequest : Shared.Contracts.Request.LeadRequest, IRequest<ActionResult<LeadResponse>>
    {

    }
}
