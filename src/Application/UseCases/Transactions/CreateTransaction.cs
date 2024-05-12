using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Entities.TransactionAggregate;
using Domain.Errors;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record CreateTransactionCommand(TransactionDto Transaction) : ICommand<Unit>;

internal class ReserveBookingCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public ReserveBookingCommandValidator()
    {
        RuleFor(t => t.Transaction.MoneyAmount).GreaterThan(.1m);
        RuleFor(t => t.Transaction.MoneyCurrency).NotEmpty();
    }
}

internal sealed class CreateTransactioCommandHandler(
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateTransactionCommand, Unit>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    public async Task<Result<Unit>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var category = await this._categoryRepository.GetByIdAsync(request.Transaction.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<Unit>(CategoryErrors.NotFound);
        }

        var transaction = Transaction.Create(
            category,
            new Money(request.Transaction.MoneyAmount, Currency.FromCode(request.Transaction.MoneyCurrency)),
            new Description(request.Transaction.Description),
            this._dateTimeProvider.UtcNow);

        await this._transactionRepository.Add(transaction.Value, cancellationToken);

        return Result.Success(Unit.Value);
    }
}