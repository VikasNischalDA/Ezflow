using FluentValidation;
using LeadManagementSystem.Comman;
using LeadManagementSystem.Shared.Contracts.Request;

namespace LeadManagementSystem.Logic.Handler
{
    public class LeadStatusUpdateRequestValidator : AbstractValidator<LeadStatusUpdateRequest>
    {
        public LeadStatusUpdateRequestValidator()
        {

            RuleFor(request => request.LeadId)
                .GreaterThan(0)
                .WithMessage("The leadid must be greater than 0.");

            RuleFor(request => request.StatusIdFrom)
                .Must(BeAValidDalasStatus)
                .WithMessage(request => $"Invalid status id from value: {request.StatusIdFrom} ");

            RuleFor(request => request.StatusIdTo)
                .Must(BeAValidDalasStatus)
                .WithMessage(request => $"Invalid status id to value: {request.StatusIdTo} ");

            RuleFor(request => request.SubStatusIdFrom)
            .Must(id => string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(id.Trim()))
           .WithMessage(request => $"Invalid sub status id from value: '{request.SubStatusIdFrom?.Trim()}'");

            RuleFor(request => request.SubStatusIdTo)
               .Must(id => string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(id.Trim()))

                .WithMessage(request => $"Invalid sub status id to value: '{request.SubStatusIdTo?.Trim()}'");



        }


        private bool BeAValidDalasStatus(int statusId)
        {
            return Enum.IsDefined(typeof(DalasStatus), statusId);
        }



    }
}