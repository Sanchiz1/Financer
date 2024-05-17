using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder.SummaryMappers;
public class MonthlySummaryMapper : SummaryMapper
{
    protected override DateOnly GetStartDate(DateOnly date) =>
        date.GetStartOfMonth();

    protected override DateOnly NextDate(DateOnly currentDate) =>
        currentDate.AddMonths(1);

    protected override decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions)
    {
        var startOfMonthDateTime = currentDate.GetStartOfMonth().ToDateTime(TimeOnly.MinValue);
        var endOfMonthDateTime = currentDate.GetEndOfMonth().ToDateTime(TimeOnly.MaxValue);

        return transactions.Where(t =>
        t.OperationDate.Date >= startOfMonthDateTime &&
        t.OperationDate.Date <= endOfMonthDateTime)
            .Sum(o => o.RealAmount);
    }

    protected override Summary CreateSummary(DateOnly currentDate, decimal amount) =>
        new Summary(
            amount,
            DateRange.Create(currentDate.GetStartOfMonth(), currentDate.GetEndOfMonth()));
}
