using AutoMapper;
using Azure.Core;
using LeadManagementSystem.Comman.Helpers;
using LeadManagementSystem.Contracts.Request;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeadManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper mapper;
        public BrandController(IMediator mediator, IMapper _mapper)
        {
            _mediator = mediator;
            mapper = _mapper;
        }

        // GET: api/<BrandController>
        [HttpGet("Brand-List")]       
        public async Task<IActionResult> Get()
        {
            try
            {                
                var response = await _mediator.Send(new BrandRequest());
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
