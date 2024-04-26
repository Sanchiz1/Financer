namespace Domain.Currencies
{
    public record CurrencyRate(
        Currency BaseCurrency,
        Currency RateCurrency,
        decimal Rate,
        DateTime Date);
}