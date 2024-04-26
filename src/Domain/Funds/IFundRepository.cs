using Domain.Currencies;

namespace Domain.Funds
{
    public interface IFundRepository
    {
        Task<Fund?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Currency> GetFundCurrency(Guid id, CancellationToken cancellationToken = default);
    }
}