using Domain.ValueObjects;
using Domain.Yahoo;

namespace Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider
{
    public sealed class ExchangeRateProviderProxy(IYahooCurrencyAPI currencyAPI) : IExchangeRateProvider
    {
        private readonly IYahooCurrencyAPI _currencyAPI = currencyAPI;
        private readonly Dictionary<(Currency, Currency), decimal> _cachedRates = [];

        public async Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
        {
            if (this._cachedRates.TryGetValue((fromCurrency, toCurrency), out var rate))
            {
                return rate;
            }
            else
            {
                decimal currencyRate = await this._currencyAPI.GetExchangeRateAsync(fromCurrency.Code, toCurrency.Code);

                var cache = new CurrencyRate(
                    fromCurrency,
                    toCurrency,
                    currencyRate,
                    DateTime.UtcNow);

                this._cachedRates.Add((fromCurrency, toCurrency), cache.Rate);

                return rate;
            }
        }
    }
}