using System.Collections.Concurrent;

namespace Domain.Yahoo
{
    public sealed class YahooCurrencyProxy(YahooCurrencyAPI currencyAPI) : IYahooCurrencyAPI
    {
        private readonly YahooCurrencyAPI _currencyAPI = currencyAPI;
        private readonly ConcurrentDictionary<(string, string), decimal> _cachedRates = [];

        public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
        {
            if (_cachedRates.TryGetValue((fromCurrencyCode, toCurrencyCode), out var rate))
            {
                return rate;
            }
            else
            {
                decimal currencyRate = await _currencyAPI.GetExchangeRateAsync(fromCurrencyCode, toCurrencyCode);

                this._cachedRates.AddOrUpdate((fromCurrencyCode, toCurrencyCode), currencyRate, (key, existingValue) => currencyRate);

                return currencyRate;

            }
        }
    }
}