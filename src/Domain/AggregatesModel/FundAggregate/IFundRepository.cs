using Domain.Entities.FundAggregate;

namespace Domain.AggregatesModel.FundAggregate;

public interface IFundRepository
{
    Task<Fund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}