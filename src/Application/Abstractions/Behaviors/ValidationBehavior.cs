using MediatR;
using FluentValidation;
using Application.Exceptions;
using Application.Abstractions.Messaging;

namespace Application.Abstractions.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!this._validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = this._validators
                .Select(validator => validator.Validate(context))
                .Where(validationResult => validationResult.Errors.Count != 0)
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new ValidationError(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage))
                .ToList();

            if (validationErrors.Count != 0)
            {
                throw new Exceptions.ValidationException(validationErrors);
            }

            return await next();
        }
    }
}
