using Infrastructure.Data;
using Domain.Entities.TransactionAggregate;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.ValueObjects;

namespace Infrastructure.Repositories;

internal sealed class TransactionRepository(ApplicationDbContext dbContext)
    : Repository<Transaction>(dbContext), ITransactionRepository
{
    public Task<IReadOnlyList<Transaction>> GetUserTransactionsAsync(
        Guid userId,
        DateRange dateRange,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Transaction>> GetUserTransactionsOfFundAsync(DateRange dateRange,
        Guid fundId,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}
