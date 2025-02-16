using AutoMapper;
using LeadManagementSystem.Comman;
using LeadManagementSystem.Comman.Helpers;
using LeadManagementSystem.Contracts.Response;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LeadRequest = LeadManagementSystem.Contracts.Request.LeadRequest;
using LeadStatusUpdateRequest = LeadManagementSystem.Contracts.Request.LeadStatusUpdateRequest;

namespace LeadManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LeadController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public LeadController(IMediator _mediator,IMapper _mapper)
        {
            mediator = _mediator;
            mapper = _mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("registerlead")]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        public async Task<IActionResult> RegisterLead([FromBody] Shared.Contracts.Request.LeadRequest request)
        {

            try
            {
                var leadRequest = MapRequest<Shared.Contracts.Request.LeadRequest, LeadRequest>(request);
                var response = await mediator.Send(leadRequest);
                if (response.Entity != null)
                {
                    var leadResponseEnvelope = LeadSoapEnveope(response.Entity);
                    return XmlJsonConverter.SerializeToXmlContentResult(leadResponseEnvelope, this);
                }
                return XmlJsonConverter.SerializeToXmlContentResult(response, this);
            }
            catch (Exception ex)
            {
                return HandleError<LeadResponse>(ex);
            }
        }


        [HttpPost]
        [Route("updateLeadStatus")]
        //[Produces("application/xml")]
        //[Consumes("application/xml")]
        /// <summary>
        /// Updates the status of an existing lead.
        /// </summary>
        /// <param name="updateRequest">Lead status update request.</param>
        /// <returns>Response indicating success or failure.</returns>

        public async Task<IActionResult> LeadStatusUpdate([FromBody] Shared.Contracts.Request.LeadStatusUpdateRequest request)
        {
            try
            {
                var leadStatusRequest = MapRequest<Shared.Contracts.Request.LeadStatusUpdateRequest, LeadStatusUpdateRequest>(request);
                var response = await mediator.Send(leadStatusRequest);
                if (response.Entity != null)
                {
                    var leadStatusUpdateResponse = leadStatusSoapEnvelope(response.Entity);
                    return Ok(leadStatusUpdateResponse.Body);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new Shared.Infrastructure.ActionResult<LeadStatusUpdateResponse>(
                    ActionResultCode.Error,
                    new List<ValidationError> { new ValidationError { FieldName = "Error", ErrorMessage = ex.Message } }
                );

                return Ok(errorResponse);
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
        private LeadResponseEnvelope LeadSoapEnveope(LeadResponse response)
        {
            return new LeadResponseEnvelope
            {
                Body = new LeadResponseBody
                {
                    CreateLeadFromRequestHDResponse = new CreateLeadFromRequestHDResponse
                    {
                        WebServiceMessage = new LeadResponse
                        {
                            Success = response.Success,
                            ErrorMessage = response.ErrorMessage,
                            LeadId = response.LeadId,
                            Decision = response.Decision,
                            Status = response.Status
                        }
                    }
                }
            };
        }

        private LeadStatusUpdateEnvelope leadStatusSoapEnvelope (LeadStatusUpdateResponse response)
        {
            return new LeadStatusUpdateEnvelope
            {
                Body = new LeadStatusUpdateBody
                {
                    LeadStatusUpdateResponse = new LeadStatusUpdateResponse
                    {
                        WebServiceMessage = new WebServiceMessage
                        {
                            Success = response.WebServiceMessage.Success,
                            ErrorMessage = response.WebServiceMessage.ErrorMessage
                        }
                    }
                }
            };
        }
    }
}
