using AutoMapper;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Data;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace LeadManagementSystem.Logic.Handler
{
    public class BrandHandler : IRequestHandler<BrandRequest, ActionResult<List<BrandResponse>>>
    {
        private readonly AppDbContext appDbContext;
        private readonly IMapper mapper;
        private readonly ILogger<DashBoardHandler> logger;

        public BrandHandler(AppDbContext _appDbContext,
                            IMapper _mapper,
                            ILogger<DashBoardHandler> _logger)
        {
            appDbContext = _appDbContext;
            mapper = _mapper;
            logger = _logger;
        }
        public async Task<ActionResult<List<BrandResponse>>> Handle(BrandRequest request, CancellationToken cancellationToken)
        {
            var result  = await appDbContext.BrandModel.ToListAsync();
            var mappedResponse = mapper.Map<List<BrandResponse>>(result);
            return new ActionResult<List<BrandResponse>> (ActionResultCode.Success,mappedResponse);            
        }
    }
}
