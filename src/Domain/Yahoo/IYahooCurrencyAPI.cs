namespace Domain.Yahoo;
public interface IYahooCurrencyAPI
{
    Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode);
}
