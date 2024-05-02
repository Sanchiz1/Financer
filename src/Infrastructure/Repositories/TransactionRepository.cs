using Infrastructure.Data;
using Domain.Entities.TransactionAggregate;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.AggregatesModel.ReportAggregate;

namespace Infrastructure.Repositories;

internal sealed class TransactionRepository(ApplicationDbContext dbContext)
    : Repository<Transaction>(dbContext), ITransactionRepository
{
    public Task<IReadOnlyList<Transaction>> GetUserTransactionsAsync(
        IUser user,
        DateRange dateRange,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Transaction>> GetUserTransactionsOfFundAsync(IUser user,
        DateRange dateRange,
        Guid fundId,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}
