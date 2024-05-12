using Domain.Abstractions;

namespace Domain.AggregatesModel.TransactionAggregate.Repositories;

public interface IRepository<T> where T : Entity<Guid>
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task Add(T entity, CancellationToken cancellationToken = default);
    Task Update(T entity, CancellationToken cancellationToken = default);
    Task Delete(T entity, CancellationToken cancellationToken = default);
}