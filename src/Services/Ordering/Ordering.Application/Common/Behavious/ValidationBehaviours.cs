using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Ordering.Application.Common.Behavious
{
    public class ValidationBehaviours<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<TRequest> _logger;

        public ValidationBehaviours(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if(!_validators.Any()) return await next();
            var context = new ValidationContext<TRequest>(request);
            var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResult.Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
            return await next();
        }
    }
}
