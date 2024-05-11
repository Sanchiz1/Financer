using Domain.ValueObjects;
using Domain.Yahoo;

namespace Domain.AggregatesModel.ReportAggregate.CurrencyConversion;
public class YahooExchangeRateAdapter : IExchangeRateProvider
{
    private readonly IYahooCurrencyAPI _yahooCurrencyAPI;

    public YahooExchangeRateAdapter(IYahooCurrencyAPI yahooCurrencyAPI)
    {
        _yahooCurrencyAPI = yahooCurrencyAPI;
    }

    public async Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
    {
        return await _yahooCurrencyAPI.GetExchangeRateAsync(fromCurrency.Code, toCurrency.Code);
    }
}
