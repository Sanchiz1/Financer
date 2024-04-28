using Domain.Shared;
using Domain.Currencies;
using Domain.Transactions;

namespace Domain.Reports
{
    public class CurrencyConversionService
    {
        private readonly Dictionary<(Currency, Currency), CurrencyRate> _currencyRates;

        public CurrencyConversionService(IEnumerable<CurrencyRate> currencyRates)
        {
            this._currencyRates = [];
            foreach (var rate in currencyRates)
            {
                this._currencyRates[(rate.BaseCurrency, rate.RateCurrency)] = rate;
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
                        transaction.CategoryId,
                        new Money(convertedAmount, targetCurrency),
                        transaction.Description,
                        transaction.UtcNow
                    );
                }
            }
        }

        public CurrencyRate GetCurrencyRate(Currency fromCurrency, Currency toCurrency)
        {
            if (this._currencyRates.TryGetValue((fromCurrency, toCurrency), out CurrencyRate rate))
            {
                return rate;
            }
            else
            {
                throw new InvalidOperationException($"Курс обміну між {fromCurrency} та {toCurrency} не знайдено.");
            }
        }
    }
}