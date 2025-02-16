using AutoMapper;
using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.Data;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LeadManagementSystem.Logic.Handler.LeadSource
{
    public class GetByUMIDHandler : IRequestHandler<LeadSourceByUMIDRequest, ActionResult<LeadSourceResponse>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetByUMIDHandler> _logger;

        public GetByUMIDHandler(AppDbContext appDbContext,
                                       IMapper mapper,
                                       ILogger<GetByUMIDHandler> logger)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult<LeadSourceResponse>> Handle(LeadSourceByUMIDRequest request, CancellationToken cancellationToken)
        {
            var query = _appDbContext.LeadSourceModels.
                Include(x => x.BrandModel).FirstOrDefault(x => x.UMID == request.UMID);

            var leadSourceResponse = _mapper.Map<LeadSourceResponse>(query);

            return new ActionResult<LeadSourceResponse>(ActionResultCode.Success, leadSourceResponse);
        }
    }
}