using AutoMapper;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.Shared.Contracts.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadSourceController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LeadSourceController(IMediator mediator, IMapper mapper) : base(mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetLeadSourceDetails")]
        public async Task<IActionResult> GetLeadSourceDetails([FromQuery] GetAllPostQuery filterRequest)
        {
            try
            {
                var result = await _mediator.Send(filterRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError<PaginationResponse>(ex);
            }
        }


        [HttpPost]
        [Route("SaveLeadSource")]
        public async Task<IActionResult> SaveLeadSource(LeadSourceRequest request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError<PaginationResponse>(ex);
            }
        }

        [HttpGet]
        [Route("GetByUMID")]
        public async Task<IActionResult> GetByUMID([FromQuery] string UMID)
        {
            try
            {
                var request = new LeadSourceByUMIDRequest { UMID = UMID };

                var result = await _mediator.Send(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError<LeadSourceResponse>(ex);
            }
        }

    }
}
