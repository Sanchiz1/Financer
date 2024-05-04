using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider;
public interface IExchangeRateProvider
{
    Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency);
}
