using Domain.Entities.TransactionAggregate;
using Domain.AggregatesModel.ReportAggregate;

namespace Domain.AggregatesModel.TransactionAggregate;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetUserTransactionsOfFundAsync(IUser user, DateRange dateRange, Guid fundId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetUserTransactionsAsync(IUser user, DateRange dateRange, CancellationToken cancellationToken = default);
}
