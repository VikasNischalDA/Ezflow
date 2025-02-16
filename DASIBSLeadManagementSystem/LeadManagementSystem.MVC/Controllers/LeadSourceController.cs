using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.MVC.Services;
using LeadManagementSystem.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LeadManagementSystem.MVC.Controllers
{
    [AuthorizeToken]
    public class LeadSourceController : Controller
    {
        private readonly ILeadSourceService _leadSourceService;
        private readonly IBrandService _brandService;

        public LeadSourceController(ILeadSourceService leadSourceService, IBrandService brandService)
        {

            _leadSourceService = leadSourceService;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _leadSourceService.GetLeadSource();
            return View(response);
        }
        
        public async Task<IActionResult> CreateLeadSource()
        {
            var items = await _brandService.GetBrands();
            var objRequest = new LeadSourceVM() { BrandList = items };
            return View(objRequest);            
        }

        public async Task<IActionResult> Edit([FromQuery] string id)
        {
            var response = await _leadSourceService.GetLeadSourceByUMID(id);
            return View(response.Entity);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditLeadSource([FromBody] LeadSourceRequest model)
        {
            var response = await _leadSourceService.UpdateLeadSource(model);

            return Json(new { success = true, redirectUrl = Url.Action("Index", "LeadSource") });

        }

        [HttpPost]
        public async Task<IActionResult> SaveLeadSource([FromBody] LeadSourceRequest model)
        {
            var response = await _leadSourceService.UpdateLeadSource(model);
            return Json(new { success = true, redirectUrl = Url.Action("Index", "LeadSource") });

        }

        [HttpGet]
        public async Task<IActionResult> GetLeadSource([FromQuery] string? name, [FromQuery] string? umid, [FromQuery] string technicalClass)
        {
           
            var response = await _leadSourceService.GetFilterLeadSource(name, umid, technicalClass);
            return Json(response.Entity);
        }
    }
}
