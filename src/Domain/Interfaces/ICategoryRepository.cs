using Domain.Entities.TransactionAggregate;

namespace Domain.Interfaces;

public interface ICategoryRepository
{
    Task<TransactionCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
