namespace Domain.Yahoo
{
    public sealed class YahooCurrencyProxy(YahooCurrencyAPI currencyAPI) : IYahooCurrencyAPI
    {
        private readonly YahooCurrencyAPI _currencyAPI = currencyAPI;
        private readonly Dictionary<(string, string), decimal> _cachedRates = [];

        public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
        {
            if (_cachedRates.TryGetValue((fromCurrencyCode, toCurrencyCode), out var rate))
            {
                return rate;
            }
            else
            {
                decimal currencyRate = await _currencyAPI.GetExchangeRateAsync(fromCurrencyCode, toCurrencyCode);

                _cachedRates.Add((fromCurrencyCode, toCurrencyCode), currencyRate);

                return rate;
            }
        }
    }
}