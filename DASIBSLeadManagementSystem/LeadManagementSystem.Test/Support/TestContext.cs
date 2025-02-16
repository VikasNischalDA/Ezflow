using LeadManagementSystem.API.Controllers;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Data;
using LeadManagementSystem.Shared.Contracts.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using LeadResponse = LeadManagementSystem.Shared.Contracts.LeadResponse;
namespace LeadManagementSystem.Test.Support
{
    public class TestContext : IDisposable
    {
        public IServiceScope scope { get; private set; }
        public IActionResult leadXmlResult { get; set; }
        public IActionResult leadUpdateResponse { get; set; }
        public ContentResult? contentResult { get; set; }
        public LeadResponseEnvelope leadResponseEnvelope { get; set; }
        public LeadRequest leadRequest { get; set; }
        public LeadStatusUpdateRequest leadStatusUpdateRequest { get; set; }
        public string xmlContent { get; set; }
        public AppDbContext dbContext { get; private set; }
        public Contracts.Response.LeadResponse leadResponse { get; set; }
        public LeadStatusUpdateEnvelope leadStatusUpdateEnvelope { get; set; }
        public LeadStatusUpdateResponse leadStatusUpdateResponse { get; set; }
        public LeadController leadController { get; private set; }
        public Shared.Infrastructure.ActionResult<Contracts.Response.LeadResponse> response { get; set; }
        public Shared.Infrastructure.ActionResult<LeadStatusUpdateResponse> LeadStatusUpdateActionResult { get; set; }

        public TestContext()
        {
            scope = TestDependencyResolver.ServiceProvider.CreateScope();
            dbContext = scope.ServiceProvider.GetService<AppDbContext>();
            leadController = scope.ServiceProvider.GetService<LeadController>();
        }

        public void Dispose()
        {
            dbContext?.Dispose();
            scope?.Dispose();
        }
    }
}