using Domain.Exceptions;
using System.Globalization;
using System.Text.Json.Nodes;

namespace Domain.Yahoo;
public class YahooCurrencyAPI : IYahooCurrencyAPI
{
    private const string ApiUrl = "https://query1.finance.yahoo.com/v8/finance/chart/";
    private readonly HttpClient _httpClient;

    public YahooCurrencyAPI(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fromCurrencyCode, nameof(fromCurrencyCode));
        ArgumentException.ThrowIfNullOrWhiteSpace(toCurrencyCode, nameof(toCurrencyCode));

        string requestUrl = $"{ApiUrl}{fromCurrencyCode.ToUpper()}{toCurrencyCode.ToUpper()}=X?interval=1d&range=1d";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        string jsonString = await response.Content.ReadAsStringAsync();
        decimal? rate = ParseExchangeRateFromJson(jsonString);

        if (rate is null)
        {
            throw new YahooException("Failed to parse data");
        }

        return rate.Value;
    }

    private static decimal? ParseExchangeRateFromJson(string jsonString)
    {
        JsonNode node = JsonNode.Parse(jsonString);

        JsonNode? chartNode = node?["chart"];
        JsonNode? resultNode = chartNode?["result"];
        JsonNode? metaNode = resultNode?[0]?["meta"];
        JsonNode? regularMarketPriceNode = metaNode?["regularMarketPrice"];

        if (regularMarketPriceNode is null)
        {
            return null;
        }

        string rateString = regularMarketPriceNode.ToString();
        
        if (decimal.TryParse(rateString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rate))
        {
            return rate;
        }

        return null;
    }
}