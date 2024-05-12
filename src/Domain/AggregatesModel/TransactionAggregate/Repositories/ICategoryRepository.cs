using Domain.Entities.TransactionAggregate;

namespace Domain.AggregatesModel.TransactionAggregate.Repositories;
public interface ICategoryRepository
{
    Task<TransactionCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}