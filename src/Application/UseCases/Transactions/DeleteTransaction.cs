using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Errors;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record DeleteTransactionCommand(string UserId, Guid TransactionId) : ICommand<Unit>;

internal sealed class DeleteTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    ITransactionAuthorizationService transactionAuthorizationService)
    : ICommandHandler<DeleteTransactionCommand, Unit>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ITransactionAuthorizationService _transactionAuthorizationService = transactionAuthorizationService;

    public async Task<Result<Unit>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        bool isAccessible = await this._transactionAuthorizationService.IsTransactionAccessible(
           request.UserId,
           request.TransactionId,
           cancellationToken);

        if (!isAccessible)
        {
            return Result.Failure<Unit>(TransactionErrors.InvalidUser);
        }

        var transaction = await this._transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is null)
        {
            return Result.Failure<Unit>(TransactionErrors.NotFound);
        }

        await this._transactionRepository.Delete(transaction, cancellationToken);

        return Result.Success(Unit.Value);
    }
}