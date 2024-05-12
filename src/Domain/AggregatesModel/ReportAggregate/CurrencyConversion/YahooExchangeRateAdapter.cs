using Domain.ValueObjects;
using Domain.Yahoo;
using System.Collections.Concurrent;

namespace Domain.AggregatesModel.ReportAggregate.CurrencyConversion;
public class YahooExchangeRateAdapter(IYahooCurrencyAPI yahooCurrencyAPI) : IExchangeRateProvider
{
    private readonly IYahooCurrencyAPI _yahooCurrencyAPI = yahooCurrencyAPI;

    private readonly ConcurrentDictionary<(Currency, Currency), decimal> _cachedRates = [];

    public async Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
    {
        if (this._cachedRates.TryGetValue((fromCurrency, toCurrency), out var rate))
        {
            return rate;
        }
        else
        {
            decimal currencyRate = await this._yahooCurrencyAPI.GetExchangeRateAsync(fromCurrency.Code, toCurrency.Code);

            this._cachedRates.AddOrUpdate((fromCurrency, toCurrency), currencyRate, (key, existingValue) => currencyRate);

            return currencyRate;
        }
    }
}