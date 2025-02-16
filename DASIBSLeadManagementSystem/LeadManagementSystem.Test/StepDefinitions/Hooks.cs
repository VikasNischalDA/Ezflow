using LeadManagementSystem.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace LeadManagementSystem.Test.StepDefinitions
{
    [Binding]
    public class Hooks
    {
        private readonly IServiceScope scope;
        private readonly AppDbContext dbContext;
        private IDbContextTransaction? transaction;

        public Hooks()
        {
            scope = TestDependencyResolver.ServiceProvider.CreateScope();
            dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        }

        [BeforeScenario]
        public async Task BeforeScenarioAsync()
        {
            // Start a new transaction before each scenario
            transaction = await dbContext.Database.BeginTransactionAsync();
        }

        [AfterScenario]
        public async Task AfterScenarioAsync()
        {
            // Rollback transaction after each scenario
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                transaction.Dispose();
            }

            // Dispose DbContext
            await dbContext.DisposeAsync();

            // Dispose scope
            scope.Dispose();
        }
    }

}
