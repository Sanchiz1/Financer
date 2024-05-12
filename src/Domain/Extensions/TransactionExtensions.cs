using Domain.AggregatesModel.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.Extensions;
public static class TransactionExtensions
{
    public static DateRange GetDateRange(this IEnumerable<Transaction> transactions)
    {
        var minDate = transactions.Min(t => t.OperationDate);
        var maxDate = transactions.Max(t => t.OperationDate);

        return DateRange.Create(
            DateOnly.FromDateTime(minDate),
            DateOnly.FromDateTime(maxDate));
    }
}
