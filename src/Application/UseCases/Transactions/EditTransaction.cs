using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Entities.TransactionAggregate;
using Domain.Errors;
using FluentValidation;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record EditTransactionCommand(string UserId, TransactionDto Transaction) : ICommand<Unit>;

internal class EditTransactionCommandValidator : AbstractValidator<EditTransactionCommand>
{
    public EditTransactionCommandValidator()
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

internal sealed class EditTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ITransactionAuthorizationService transactionAuthorizationService,
    IMapper mapper)
    : ICommandHandler<EditTransactionCommand, Unit>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ITransactionAuthorizationService _transactionAuthorizationService = transactionAuthorizationService;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<Unit>> Handle(EditTransactionCommand request, CancellationToken cancellationToken)
    {
        bool isAccessible = await this._transactionAuthorizationService.IsTransactionAccessible(
           request.UserId,
           request.Transaction.Id,
           cancellationToken);

        if (!isAccessible)
        {
            return Result.Failure<Unit>(TransactionErrors.InvalidUser);
        }


        var transaction = await this._transactionRepository.GetByIdAsync(request.Transaction.Id, cancellationToken);

        if (transaction is null)
        {
            return Result.Failure<Unit>(TransactionErrors.NotFound);
        }

        transaction = this._mapper.Map<Transaction>(request.Transaction);

        await this._transactionRepository.Update(transaction, cancellationToken);

        return Result.Success(Unit.Value);
    }
}