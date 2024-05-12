using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.TransactionAggregate.Repositories;
public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IReadOnlyList<Transaction>> GetAllAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetInDateRangeAsync(string userId, DateRange dateRange, CancellationToken cancellationToken = default);
}