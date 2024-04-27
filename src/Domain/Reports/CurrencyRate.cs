using Domain.Currencies;

namespace Domain.Reports
{
    public record CurrencyRate(
        Currency BaseCurrency,
        Currency RateCurrency,
        decimal Rate,
        DateTime Date);
}