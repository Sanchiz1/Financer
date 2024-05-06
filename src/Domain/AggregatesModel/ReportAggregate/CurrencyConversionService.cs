using Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider;
using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate;
public class CurrencyConversionService(IExchangeRateProvider providerProxy)
{
    private readonly IExchangeRateProvider _providerProxy = providerProxy;

    public async Task<IEnumerable<Transaction>> ConvertTransactionsAsync(IEnumerable<Transaction> transactions, Currency targetCurrency)
    {
        List<Transaction> convertedTransactions = [];
        foreach (var transaction in transactions)
        {
            if (transaction.Amount.Currency == targetCurrency)
            {
                convertedTransactions.Add(transaction);
            }
            else
            {
                convertedTransactions.Add(await ConvertTransactionAsync(transaction, targetCurrency));
            }
        }
        return convertedTransactions;
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