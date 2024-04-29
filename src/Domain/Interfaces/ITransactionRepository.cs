using Domain.Users;
using Domain.Reports;
using Domain.Entities.TransactionAggregate;

namespace Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetUserTransactionsOfFundAsync(IUser user, DateRange dateRange, Guid fundId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetUserTransactionsAsync(IUser user, DateRange dateRange, CancellationToken cancellationToken = default);
}
