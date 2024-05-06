using Domain.ValueObjects;
using Domain.Yahoo;

namespace Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider;
public class YahooExchangeRateAdapter : IExchangeRateProvider
{
    private readonly YahooCurrencyAPI _yahooCurrencyAPI;

    public YahooExchangeRateAdapter(YahooCurrencyAPI yahooCurrencyAPI)
    {
        _yahooCurrencyAPI = yahooCurrencyAPI;
    }

    public async Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
    {
        return await _yahooCurrencyAPI.GetExchangeRateAsync(fromCurrency.Code, toCurrency.Code);
    }
}
