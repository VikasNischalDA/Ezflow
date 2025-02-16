using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;

namespace LeadManagementSystem.MVC.Services
{
    public interface ILeadSourceService
    {
        Task<ActionResult<BaseAPIResponse>> GetLeadSource();
        Task<ActionResult<LeadSourceResponse>> GetLeadSourceByUMID(string UMID);
        Task<ActionResult<bool>> UpdateLeadSource(LeadSourceRequest model);
        Task<ActionResult<BaseAPIResponse>> GetFilterLeadSource(string name, string umid, string technicalClass);
    }
}
