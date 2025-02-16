using AutoMapper;
using LeadManagementSystem.Contracts.Request.LeadSource;
using LeadManagementSystem.Data;
using LeadManagementSystem.Model.Models;
using LeadManagementSystem.Shared.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LeadManagementSystem.Logic.Handler.LeadSource
{
    public class LeadSourceHandler : IRequestHandler<LeadSourceRequest, ActionResult<bool>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<LeadSourceHandler> _logger;

        public LeadSourceHandler(AppDbContext appDbContext,
                                       IMapper mapper,
                                       ILogger<LeadSourceHandler> logger)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult<bool>> Handle(LeadSourceRequest request, CancellationToken cancellationToken)
        {
            try
            {


                if (request != null && request.Id.HasValue)
                {
                    return await updateLeadSource(request);
                }
                else
                {
                    return await saveLeadSource(request);
                }
            }
            catch(Exception ex)
            {
                return new ActionResult<bool>(ActionResultCode.Error, false);
            }
        }

        private async Task<ActionResult<bool>> saveLeadSource(LeadSourceRequest? request)
        {
            request.DateTime = DateTime.Now;
            var modelToSave = mapRequestToModel(request);
            if (modelToSave != null)
            {
                try
                {                    
                    _appDbContext.LeadSourceModels.Add(modelToSave);
                    await _appDbContext.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return new ActionResult<bool>(ActionResultCode.Success, true);
        }


        private async Task<ActionResult<bool>> updateLeadSource(LeadSourceRequest request)
        {
            var existingLeadSource = _appDbContext.LeadSourceModels.Where(x => x.Id == request.Id).FirstOrDefault();
            if (existingLeadSource != null)
            {
                 mapRequestToExistingModel(existingLeadSource,request);
                _appDbContext.Update(existingLeadSource);
                await _appDbContext.SaveChangesAsync(true);
            }
            return new ActionResult<bool>(ActionResultCode.Success, true);
        }

        private void mapRequestToExistingModel(LeadSourceModel existingLeadSource, LeadSourceRequest request)
        {
            existingLeadSource.DigitalLeadSource = request.DigitalLeadSource;
            existingLeadSource.LeadSourceClass = request.LeadSourceClass;
            existingLeadSource.Active = request.Active;
            existingLeadSource.BusinessUnit = request.BusinessUnit;           
            existingLeadSource.BrandId = request.BrandId;
            existingLeadSource.ChangedBy = request.ChangedBy;
            existingLeadSource.CustomerFriendlyName = request.CustomerFriendlyName;
            existingLeadSource.LeadSource = request.LeadSource;
            existingLeadSource.ModifiedDate = DateTime.Now;            
            existingLeadSource.TelephoneNumber = request.TelephoneNumber;
            existingLeadSource.UMID = request.UMID;
        }

        private LeadSourceModel mapRequestToModel(LeadSourceRequest? request)
        {
            return _mapper.Map<LeadSourceModel>(request);
        }
    }
}
