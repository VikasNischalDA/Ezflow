using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;

namespace LeadManagementSystem.MVC.Services
{
    public interface IQueueManagementService
    {
        Task<ActionResult<QueueManagementViewModel>> GetLeadRequest();
    }
}
