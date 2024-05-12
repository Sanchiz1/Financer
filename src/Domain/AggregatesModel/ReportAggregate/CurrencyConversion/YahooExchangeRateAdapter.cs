using Domain.ValueObjects;
using Domain.Yahoo;

namespace Domain.AggregatesModel.ReportAggregate.CurrencyConversion;
public class YahooExchangeRateAdapter(IYahooCurrencyAPI yahooCurrencyAPI) : IExchangeRateProvider
{
    private readonly IYahooCurrencyAPI _yahooCurrencyAPI = yahooCurrencyAPI;

    public async Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
    {
        return await this._yahooCurrencyAPI.GetExchangeRateAsync(fromCurrency.Code, toCurrency.Code);
    }
}