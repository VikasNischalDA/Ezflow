using AutoMapper;
using LeadManagementSystem.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace LeadManagementSystem.API.Controllers
{
    public class BaseController : ControllerBase
    {
        private IMapper _mapper;
        public BaseController(IMapper mapper)
        {
            _mapper = mapper;  
        }
        protected IActionResult HandleError<T>(Exception ex)
        {
            var errorResponse = new Shared.Infrastructure.ActionResult<T>(
                ActionResultCode.Error,
                new List<ValidationError> { new ValidationError { FieldName = "Error", ErrorMessage = ex.Message } }
            );
            return Ok(errorResponse);
        }
        protected TDestination MapRequest<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }
    }
}
