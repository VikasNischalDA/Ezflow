using FluentValidation;
using LeadManagementSystem.Shared.Extensions;
using MediatR;

namespace LeadManagementSystem.Shared.Infrastructure
{
    public class ValidatorHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse> where TResponse : ActionResult
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorHandler(IRequestHandler<TRequest, TResponse> inner,
            IValidator<TRequest>[] validators)
        {
            _inner = inner;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f.NotNull())
                .ToList();
            if (failures.Any())
            {
                var response = typeof(TResponse);
                if (response.IsGenericType)
                    return
                        Activator.CreateInstance(response, ActionResultCode.Error, failures.Select(x => { return new ValidationError() { ErrorMessage = x.ErrorMessage, FieldName = x.PropertyName }; }).ToList(),
                            null) as TResponse;
                return
                    Activator.CreateInstance(response, ActionResultCode.ValidationFailed, failures.Select(x => { return new ValidationError() { ErrorMessage = x.ErrorMessage, FieldName = x.PropertyName }; }).ToList()) as
                        TResponse;
            }
            return await _inner.Handle(request, cancellationToken);
        }
    }
}
