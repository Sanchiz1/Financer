using Domain.Users;
using Domain.Reports;

namespace Domain.Transactions
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Transaction>> GetUserTransactionsOfFundAsync(User user, DateRange dateRange, Guid fundId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Transaction>> GetUserTransactionsAsync(User user, DateRange dateRange, CancellationToken cancellationToken = default);
    }
}
