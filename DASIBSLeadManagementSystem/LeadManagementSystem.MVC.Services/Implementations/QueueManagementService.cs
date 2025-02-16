using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.MVC.Services;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using LeadManagementSystem.Shared.Contracts.Request;
using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadRequest = LeadManagementSystem.Shared.Contracts.Request.LeadRequest;

namespace LeadManagementSystem.MVC.Services
{
    public class QueueManagementService : IQueueManagementService
    {
        public async Task<ActionResult<QueueManagementViewModel>> GetLeadRequest()
        {
            var a = new ActionResult<QueueManagementViewModel>
            {
                Entity = new QueueManagementViewModel
                {
                    AllLeads = new List<LeadRequest>
            {
                new LeadRequest
                {
                    Id = 12154,
                    SupplierSource = "12542",
                    Surname = "jones",
                    IDNumber = "1236545236512"
                }
            }
                }
            };
            return a;
        }
    }
}
