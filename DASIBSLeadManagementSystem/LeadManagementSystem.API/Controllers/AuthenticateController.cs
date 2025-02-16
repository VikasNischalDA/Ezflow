using AutoMapper;
using LeadManagementSystem.Comman.Helpers;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace LeadManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        public AuthenticateController(IMediator _mediator, IMapper _mapper)
        {
            mediator = _mediator;
            mapper = _mapper;
        }

        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Shared.Contracts.Request.LoginRequest request)
        {
            try
            {
                var leadRequest = MapRequest<Shared.Contracts.Request.LoginRequest, LoginRequest>(request);
                var response = await mediator.Send(leadRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleError<LeadResponse>(ex);
            }
        }

        private TDestination MapRequest<TSource, TDestination>(TSource source)
        {
            return mapper.Map<TDestination>(source);
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
