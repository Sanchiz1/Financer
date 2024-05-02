using Domain.Exceptions;
using System.Text.Json.Nodes;

namespace Domain.Yahoo;
public class YahooCurrencyAPI : IYahooCurrencyAPI
{
    private static string apiUrl = "https://query1.finance.yahoo.com/v8/finance/chart/USDEUR=X?interval=1d&range=1d";
    private readonly HttpClient _httpClient;

    public YahooCurrencyAPI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode) throw new YahooException("Failed to fetch data");

        string jsonString = await response.Content.ReadAsStringAsync();

        var node = JsonNode.Parse(jsonString)!;

        var rate = node["chart"]?["result"]?[0]?["meta"]?["regularMarketPrice"]?.ToString();

        if (rate is null)
        {
            throw new YahooException("Failed to parse data");
        }

        return decimal.Parse(rate);
    }

}
