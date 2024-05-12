using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.CurrencyConversion;
public interface IExchangeRateProvider
{
    Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency);
}
