using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider;

public interface IExchangeRateProvider
{
    Task<decimal> GetExchangeRate(Currency fromCurrency, Currency toCurrency);
}
