namespace Domain.Currencies
{
    public interface ICurrencyRepository
    {
        Task<Currency?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default); 
    }
}
