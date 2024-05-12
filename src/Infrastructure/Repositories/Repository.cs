using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Common;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal abstract class Repository<T>(ApplicationDbContext dbContext) : IRepository<T> where T : Entity<Guid>
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.DbContext
            .Set<T>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this.DbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public virtual async Task Add(T entity, CancellationToken cancellationToken = default)
    {
        this.DbContext.Add(entity);
        await this.DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task Update(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Update(entity);
        await this.DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task Delete(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Remove(entity);
        await this.DbContext.SaveChangesAsync(cancellationToken);
    }

}