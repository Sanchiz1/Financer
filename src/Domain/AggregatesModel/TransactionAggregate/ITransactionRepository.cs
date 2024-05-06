using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.TransactionAggregate;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetUserTransactionsOfFundAsync(DateRange dateRange, Guid fundId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetUserTransactionsAsync(DateRange dateRange, CancellationToken cancellationToken = default);
}
