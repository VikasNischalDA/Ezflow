using AutoMapper;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Data;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LeadManagementSystem.Logic.Handler
{
    public class GetLeadSourceHandler : IRequestHandler<GetAllPostQuery, ActionResult<BaseAPIResponse>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetLeadSourceHandler> _logger;

        public GetLeadSourceHandler(AppDbContext appDbContext,
                                       IMapper mapper,
                                       ILogger<GetLeadSourceHandler> logger)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<ActionResult<BaseAPIResponse>> Handle(GetAllPostQuery filter, CancellationToken cancellationToken)
        {
            BaseAPIResponse response = new BaseAPIResponse();
            response.ListOfLeadSource = new List<LeadSourceResponse>();
            var queryable = _appDbContext.LeadSourceModels.AsQueryable();

            if (!string.IsNullOrEmpty(filter?.Name))
            {
                queryable = queryable.Where(x => x.CustomerFriendlyName.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter?.Class))
            {
                if (filter.Class.Equals("External Vendor", StringComparison.OrdinalIgnoreCase))
                {
                    queryable = queryable.Where(x => x.LeadSourceClass == "External Vendor");
                }
                else if (filter.Class.Equals("All SIBS", StringComparison.OrdinalIgnoreCase))
                {
                    // Include both "External Vendor" and any other BusinessUnit values
                    queryable = queryable.Where(x => x.LeadSourceClass == "External Vendor" || x.LeadSourceClass != "External Vendor");
                }
            }
            if (!string.IsNullOrEmpty(filter?.UMID))
            {
                queryable = queryable.Where(x => x.UMID.Contains(filter.UMID));
            }
            
            var result = await queryable.ToListAsync();
            var mappedResponse  = _mapper.Map<List<LeadSourceResponse>>(result);
            response.ListOfLeadSource = mappedResponse; 
            return new ActionResult<BaseAPIResponse>(ActionResultCode.Success, response);
        }
    }
}
