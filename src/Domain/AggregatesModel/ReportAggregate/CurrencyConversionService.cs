using Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider;
using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate;
public class CurrencyConversionService(ExchangeRateProviderProxy providerProxy)
{
    private readonly ExchangeRateProviderProxy _providerProxy = providerProxy;

    public async IAsyncEnumerable<Transaction> ConvertTransactionsAsync(IEnumerable<Transaction> transactions, Currency targetCurrency)
    {
        foreach (var transaction in transactions)
        {
            if (transaction.Amount.Currency == targetCurrency)
            {
                yield return transaction;
            }
            else
            {
                yield return await ConvertTransactionAsync(transaction, targetCurrency);
            }
        }
    }

    private async Task<Transaction> ConvertTransactionAsync(Transaction transaction, Currency targetCurrency)
    {
        var exchangeRate = await _providerProxy.GetExchangeRateAsync(transaction.Amount.Currency, targetCurrency);
        var convertedAmount = transaction.Amount * exchangeRate;

        return new Transaction(
            transaction.Id,
            transaction.FundId,
            transaction.Category,
            new Money(convertedAmount, targetCurrency),
            transaction.Description,
            transaction.OperationDate
        );
    }
}