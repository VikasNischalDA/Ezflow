using LeadManagementSystem.Data;
using LeadManagementSystem.Model.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

public static class DatabaseSeeder
{
    public static void SeedDatabase(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var leadResponses = new[]
            {
            new LeadResponseModel
            {
                LeadId = 216,
                ErrorMessage = null,
                BrandApplicationId = "BRAND_APP_001",
                ApplicationId = "APP_001",
                BrandId = "BRAND_001",
                Decision = "Approved",
                Status = "Completed",
                LesResponse = "LES_RESPONSE_001",
                DbsResponse = "DBS_RESPONSE_001",
                TurboResponse = "TURBO_RESPONSE_001"
            },
            new LeadResponseModel
            {
                LeadId = 217,
                ErrorMessage = "Invalid application data",
                BrandApplicationId = "BRAND_APP_002",
                ApplicationId = "APP_002",
                BrandId = "BRAND_002",
                Decision = "Declined",
                Status = "Error",
                LesResponse = "LES_RESPONSE_002",
                DbsResponse = "DBS_RESPONSE_002",
                TurboResponse = null
            }
        };

            foreach (var leadResponse in leadResponses)
            {
                if (!dbContext.LeadResponseModel.Any(l => l.LeadId == leadResponse.LeadId))
                {
                    dbContext.LeadResponseModel.Add(leadResponse);
                }
            }

            var leadStatuses = new[]
            {
            new LeadStatusModel
            {
                LeadId = 216,
                StatusIdFrom = "ManageResponse",
                SubStatusIdFrom = "PreQualification",
                StatusIdTo = "ClientToCall",
                SubStatusIdTo = "ClientCancelled",
                System = "Dalas",
                DalasUserName = "user123",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new LeadStatusModel
            {
                LeadId = 217,
                StatusIdFrom = "LESApproved",
                SubStatusIdFrom = "NoApp",
                StatusIdTo = "NotTakenUpPending",
                SubStatusIdTo = "ClientToCall",
                System = "Dalas",
                DalasUserName = "user456",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        };

            foreach (var leadStatus in leadStatuses)
            {
                if (!dbContext.LeadStatusModel.Any(ls => ls.LeadId == leadStatus.LeadId))
                {
                    dbContext.LeadStatusModel.Add(leadStatus);
                }
            }


            dbContext.SaveChanges();
        }
    }
}