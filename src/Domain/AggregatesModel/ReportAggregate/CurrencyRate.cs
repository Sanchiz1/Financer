using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate;

public record CurrencyRate(
    Currency BaseCurrency,
    Currency RateCurrency,
    decimal Rate,
    DateTime Date);