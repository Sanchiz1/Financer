using Domain.ValueObjects;

namespace Domain.Reports;

public record CurrencyRate(
    Currency BaseCurrency,
    Currency RateCurrency,
    decimal Rate,
    DateTime Date);