using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;

namespace LeadManagementSystem.MVC.Services
{
    public interface IDashboardService
    {
        Task<ActionResult<PaginationResponse>> GetLeads();
        Task<ActionResult<PaginationResponse>> GetFilterLeadRecords(string? className, int? pageSize, int? pageNumber);
    }
}
