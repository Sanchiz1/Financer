using Application.Common.Interfaces;
using Domain.AggregatesModel.TransactionAggregate.Repositories;

namespace Infrastructure.Common;
internal sealed class TransactionAuthorizationService(ITransactionRepository transactionRepository) : ITransactionAuthorizationService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task<bool> IsTransactionAccessible(string userId, Guid transactionId, CancellationToken cancellationToken = default)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId, cancellationToken);
        return transaction?.Category.UserId.ToString() == userId;
    }
}
