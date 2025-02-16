using LeadManagementSystem.Shared.Contracts.Request;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;

namespace LeadManagementSystem.MVC.Services
{
    public interface IAuthenticationService
    {
        Task<ActionResult<LoginResponse>> AuthenticateAsync(LoginRequest loginDetails);
    }
}
