using Domain.Entities.FundAggregate;

namespace Domain.Interfaces;

public interface IFundRepository
{
    Task<Fund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}