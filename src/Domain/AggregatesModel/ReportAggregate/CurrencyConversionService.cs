using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate;

public class CurrencyConversionService
{
    private readonly Dictionary<(Currency, Currency), CurrencyRate> _currencyRates;

    public CurrencyConversionService(IEnumerable<CurrencyRate> currencyRates)
    {
        _currencyRates = [];
        foreach (var rate in currencyRates)
        {
            _currencyRates[(rate.BaseCurrency, rate.RateCurrency)] = rate;
        }
    }

    public IEnumerable<Transaction> ConvertTransactions(IEnumerable<Transaction> transactions, Currency targetCurrency)
    {
        foreach (var transaction in transactions)
        {
            if (transaction.Amount.Currency == targetCurrency)
            {
                yield return transaction;
            }
            else
            {
                CurrencyRate rate = GetCurrencyRate(transaction.Amount.Currency, targetCurrency);

                decimal convertedAmount = transaction.Amount * rate.Rate;

                yield return new Transaction(
                    transaction.Id,
                    transaction.FundId,
                    transaction.Category,
                    new Money(convertedAmount, targetCurrency),
                    transaction.Description,
                    transaction.OperationDate
                );
            }
        }
    }

    public CurrencyRate GetCurrencyRate(Currency fromCurrency, Currency toCurrency)
    {
        if (_currencyRates.TryGetValue((fromCurrency, toCurrency), out CurrencyRate rate))
        {
            return rate;
        }
        else
        {
            // TODO: get rate from IExchangeRateProvider and cache
            throw new InvalidOperationException($"Курс обміну між {fromCurrency} та {toCurrency} не знайдено.");
        }
    }
}