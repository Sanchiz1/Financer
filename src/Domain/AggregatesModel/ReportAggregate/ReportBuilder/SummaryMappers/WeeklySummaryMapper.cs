using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder.SummaryMappers;
public class WeeklySummaryMapper : SummaryMapper
{
    protected override DateOnly GetStartDate(DateOnly date) => 
        date.GetStartOfWeek();
    
    protected override DateOnly NextDate(DateOnly currentDate) =>
        currentDate.AddDays(7);

    protected override decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions)
    {
        var startOfWeekDateTime = currentDate.GetStartOfWeek().ToDateTime(TimeOnly.MinValue);
        var endOfWeekDateTime = currentDate.GetEndOfWeek().ToDateTime(TimeOnly.MaxValue);

        return transactions.Where(t =>
        t.OperationDate.Date >= startOfWeekDateTime &&
        t.OperationDate.Date <= endOfWeekDateTime)
            .Sum(o => o.RealAmount);
    }

    protected override Summary CreateSummary(DateOnly currentDate, decimal amount) =>
        new Summary(
                amount,
                DateRange.Create(currentDate.GetStartOfWeek(), currentDate.GetEndOfWeek()));
}
