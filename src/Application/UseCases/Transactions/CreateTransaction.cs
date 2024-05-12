using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Errors;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record CreateTransactionCommand(TransactionDto Transaction) : ICommand<Unit>;

internal class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(cmd => cmd.Transaction)
            .NotNull().WithMessage("Transaction details must not be null.");

        RuleFor(cmd => cmd.Transaction.MoneyAmount)
            .GreaterThan(0).WithMessage("Money amount must be greater than zero.");

        RuleFor(cmd => cmd.Transaction.MoneyCurrency)
            .NotEmpty().WithMessage("Money currency must not be empty.")
            .Length(3).WithMessage("Money currency must be 3 characters long.");

        RuleFor(cmd => cmd.Transaction.CategoryId)
            .NotNull().WithMessage("Transaction category id must not be null.");
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