using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.CurrencyConversion;

public record CurrencyRate(
    Currency BaseCurrency,
    Currency RateCurrency,
    decimal Rate,
    DateTime Date);