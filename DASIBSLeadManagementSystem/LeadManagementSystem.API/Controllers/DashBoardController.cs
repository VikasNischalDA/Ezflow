using AutoMapper;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Shared.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public DashBoardController(IMediator _mediator, IMapper _mapper) : base(_mapper)
        {
            mediator = _mediator;
            mapper = _mapper;
        }

        [HttpGet]
        [Route("leadRecords")]
        public async Task<IActionResult> GetLeadRecords([FromQuery] Shared.Contracts.Request.LeadRecordRequest paginationParams)
        {
            try
            {
                // Validate PageSize
                if (paginationParams.PageSize < 1)
                    return BadRequest("Page size must be greater than 0.");

                var leadRecordRequest = MapRequest<Shared.Contracts.Request.LeadRecordRequest, LeadRecordRequest>(paginationParams);

                var result = await mediator.Send(leadRecordRequest);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError<PaginationResponse>(ex);
            }
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IActionResult> GetFilteredLeadSources([FromQuery] LeadRecordRequest leadRecordRequest, [FromQuery] string classType)
        {
            try
            {
                if (string.IsNullOrEmpty(classType))
                    return BadRequest("Class type is required.");

                var filterLeadRequest = MapRequest<Shared.Contracts.Request.LeadRecordRequest, LeadRecordRequest>(leadRecordRequest);

                filterLeadRequest.ClassType = classType;

                var result = await mediator.Send(filterLeadRequest);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError<PaginationResponse>(ex);
            }
        }




        private IActionResult HandleError<T>(Exception ex)
        {
            var errorResponse = new Shared.Infrastructure.ActionResult<T>(
                ActionResultCode.Error,
                new List<ValidationError> { new ValidationError { FieldName = "Error", ErrorMessage = ex.Message } }
            );
            return Ok(errorResponse);
        }


    }
}
