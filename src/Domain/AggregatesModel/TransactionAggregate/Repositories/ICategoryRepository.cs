namespace Domain.AggregatesModel.TransactionAggregate.Repositories;
public interface ICategoryRepository : IRepository<TransactionCategory>
{
    Task<TransactionCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}