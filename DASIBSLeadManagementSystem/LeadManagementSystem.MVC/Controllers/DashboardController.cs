using LeadManagementSystem.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSystem.MVC.Controllers
{
    [AuthorizeToken]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {

            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _dashboardService.GetLeads();

            ViewBag.TotalLeadReceived = response.Entity.Data?.Sum(r => r.LeadReceived) ?? 0;
            ViewBag.TotalLeadsNotProcessed = response.Entity.Data?.Sum(r => r.LeadsNotProcessed) ?? 0;
            ViewBag.TotalAppCat = response.Entity.Data?.Sum(r => r.AppCapture) ?? 0;
            ViewBag.TotalCreditApps = response.Entity.Data?.Sum(r => r.CreditApps) ?? 0;
            ViewBag.TotalValidationApps = response.Entity.Data?.Sum(r => r.ValidationApps) ?? 0;
            ViewBag.TotalFraudApps = response.Entity.Data?.Sum(r => r.FraudApps) ?? 0;
            ViewBag.TotalContractApplications = response.Entity.Data?.Sum(r => r.ContractApplications) ?? 0;

            ViewBag.TotalCount = response.Entity.TotalCount;
            ViewBag.CurrentPage = response.Entity.CurrentPage;
            ViewBag.PageSize = response.Entity.PageSize;
            ViewBag.HasNextPage = response.Entity.HasNextPage;
            ViewBag.HasPreviousPage = response.Entity.HasPreviousPage;

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> RefreshDashboard(string? className, int? pageSize, int pageNumber )
        {
            var response = await _dashboardService.GetFilterLeadRecords(className, pageSize, pageNumber);

            var data = new
            {
                TotalLeadReceived = response.Entity.Data?.Sum(r => r.LeadReceived) ?? 0,
                TotalLeadsNotProcessed = response.Entity.Data?.Sum(r => r.LeadsNotProcessed) ?? 0,
                TotalAppCat = response.Entity.Data?.Sum(r => r.AppCapture) ?? 0,
                TotalCreditApps = response.Entity.Data?.Sum(r => r.CreditApps) ?? 0,
                TotalValidationApps = response.Entity.Data?.Sum(r => r.ValidationApps) ?? 0,
                TotalFraudApps = response.Entity.Data?.Sum(r => r.FraudApps) ?? 0,
                TotalContractApplications = response.Entity.Data?.Sum(r => r.ContractApplications) ?? 0,
                TotalCount = response.Entity.TotalCount,
                CurrentPage = response.Entity.CurrentPage,
                PageSize = response.Entity.PageSize,
                HasNextPage = response.Entity.HasNextPage,
                HasPreviousPage = response.Entity.HasPreviousPage

        };

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilterLeadRecords(string? className, int? pageSize, int? pageNumber)
        {
            var response = await _dashboardService.GetFilterLeadRecords(className, pageSize, pageNumber);
           
            var data = response.Entity;

            return Json(data);
        }
    }
}
