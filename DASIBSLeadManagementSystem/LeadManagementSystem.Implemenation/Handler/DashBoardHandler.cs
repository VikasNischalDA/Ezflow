using AutoMapper;
using LeadManagementSystem.Data;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LeadRecordRequest = LeadManagementSystem.Contracts.Request.LeadRecordRequest;


namespace LeadManagementSystem.Logic.Handler
{
    public class DashBoardHandler : IRequestHandler<LeadRecordRequest, ActionResult<PaginationResponse>>
    {
        private readonly AppDbContext appDbContext;
        private readonly IMapper mapper;
        private readonly ILogger<DashBoardHandler> logger;

        public DashBoardHandler(AppDbContext _appDbContext,
                                       IMapper _mapper,
                                       ILogger<DashBoardHandler> _logger)
        {
            appDbContext = _appDbContext;
            mapper = _mapper;
            logger = _logger;
        }

        public async Task<ActionResult<PaginationResponse>> Handle(LeadRecordRequest request, CancellationToken cancellationToken)
        {
            
            var result = await GetPaginatedLeadRecordsAsync(request, cancellationToken);

            return result;
            
        }

        private async Task<ActionResult<PaginationResponse>> GetPaginatedLeadRecordsAsync(LeadRecordRequest request, CancellationToken cancellationToken)
        {
            var today =  DateTime.Today;

            var query = appDbContext.LeadSourceModels
                .Include(ls => ls.LeadResponses)
                .Include(ls => ls.LeadStatusHistories)
    .Where(x => x.Active == true);

            if (!string.IsNullOrEmpty(request.ClassType))
            {
                query = query.Where(ls => ls.Active == true &&
                    (request.ClassType == "External Vendor" ? ls.LeadSourceClass == "External Vendor" : true));

            }


            var totalCount = await query
                .GroupBy(ls => ls.UMID) // Ensuring each UMID is counted separately
                .CountAsync(cancellationToken);

            
            var pagedData = await query
                .GroupBy(ls => ls.UMID) // Grouping by UMID ensures uniqueness
                .Select(grouped => new LeadRecordResponse
                {
                    Class = grouped.First().LeadSourceClass, // Taking the first value in the group
                    LeadSource = grouped.First().LeadSource,
                    LeadReceived = grouped.SelectMany(g => g.LeadResponses)
                        .Count(lr => EF.Functions.DateDiffDay(lr.CreatedDate, today) == 0),
                    LeadsNotProcessed = grouped.SelectMany(g => g.LeadResponses)
                        .Count(lr => lr.TurboResponse == null && EF.Functions.DateDiffDay(lr.CreatedDate, today) == 0),
                    AppCapture = grouped.SelectMany(g => g.LeadStatusHistories)
                        .Count(lh => new[] { "NewApplication", "Qualification", "DebtorLookup", "AppCapture", "ManageResponse", "PreQualification" }
                        .Contains(lh.StatusIdTo) && EF.Functions.DateDiffDay(lh.CreatedDate, today) == 0),
                    CreditApps = grouped.SelectMany(g => g.LeadStatusHistories)
                        .Count(lh => lh.StatusIdTo == "CreditAssessment" && EF.Functions.DateDiffDay(lh.CreatedDate, today) == 0),
                    ValidationApps = grouped.SelectMany(g => g.LeadStatusHistories)
                        .Count(lh => lh.StatusIdTo == "Validation" && EF.Functions.DateDiffDay(lh.CreatedDate, today) == 0),
                    FraudApps = grouped.SelectMany(g => g.LeadStatusHistories)
                        .Count(lh => lh.StatusIdTo == "Fraud" && EF.Functions.DateDiffDay(lh.CreatedDate, today) == 0),
                    ContractApplications = grouped.SelectMany(g => g.LeadStatusHistories)
                        .Count(lh => lh.StatusIdTo == "Contract" && EF.Functions.DateDiffDay(lh.CreatedDate, today) == 0)
                })
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            

           
            var hasNextPage = (request.PageNumber * request.PageSize) < totalCount;
            var hasPreviousPage = request.PageNumber > 1;

            // Create the paginated response
            var results = new PaginationResponse
            {
                TotalCount = totalCount,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage,
                Data = pagedData
            };

            return new ActionResult<PaginationResponse>(ActionResultCode.Success, results);
        }
    }

}
