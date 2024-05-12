using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class TransactionRepository(ApplicationDbContext dbContext)
    : Repository<Transaction>(dbContext), ITransactionRepository
{
    public override async Task<Transaction?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        return await DbContext
           .Set<Transaction>()
           .Include(t => t.Category)
           .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetAllAsync(
        string userId, 
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Transaction>()
            .Include(t => t.Category)
            .Where(t => t.Category.UserId.ToString() == userId);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaction>> GetInDateRangeAsync(
        string userId, 
        DateRange dateRange, 
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Transaction>()
            .Include(t => t.Category)
            .Where(t => t.Category.UserId.ToString() == userId
                && DateOnly.FromDateTime(t.OperationDate) >= dateRange.Start
                && DateOnly.FromDateTime(t.OperationDate) <= dateRange.End);

        return await query.ToListAsync(cancellationToken);
    }
}