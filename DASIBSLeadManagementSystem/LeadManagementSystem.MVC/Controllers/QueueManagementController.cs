using LeadManagementSystem.MVC.Services;
using LeadManagementSystem.Shared.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace LeadManagementSystem.MVC.Controllers
{
    [AuthorizeToken]
    public class QueueManagementController : Controller
    {
        private readonly IQueueManagementService _queueManagementService;
        LeadManagementSystem.Shared.Infrastructure.ActionResult<QueueManagementViewModel> _viewModel = new Shared.Infrastructure.ActionResult<QueueManagementViewModel>();

        public QueueManagementController(IQueueManagementService queueManagementService)
        {
            _queueManagementService = queueManagementService;
        }
        public async Task<IActionResult> Index()
        {
            //var allLeads = await _queueManagementService.GetLeadRequest();
            //_viewModel.Entity = new QueueManagementViewModel();
            //_viewModel.Entity.AllLeads = allLeads.Entity.AllLeads;
            return View("QueueManagement");
        }
        [HttpGet]
        public async Task<IActionResult> LoadAllLeads()
        {
            //var allLeads = await _queueManagementService.GetLeadRequest();
            //_viewModel.Entity.AllLeads = allLeads.Entity.AllLeads;
            return PartialView("_AllLeadsPartial");
        }
        [HttpGet]
        public async Task<IActionResult> LoadAllTasks()
        {

            return PartialView("_AllTasksPartial");
        }
        [HttpGet]
        public async Task<IActionResult> LoadAllGloabalQueue()
        {

            return PartialView("_GlobalQueuePartial");
        }
    }
}
