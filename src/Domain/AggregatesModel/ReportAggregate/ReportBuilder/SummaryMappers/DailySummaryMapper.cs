using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder.SummaryMappers;
public class DailySummaryMapper : SummaryMapper
{
    protected override DateOnly GetStartDate(DateOnly date) => date;

    protected override DateOnly NextDate(DateOnly currentDate) =>
        currentDate.AddDays(1);

    protected override decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions) =>
        transactions.Where(t => t.OperationDate.IsDate(currentDate)).Sum(o => o.RealAmount);

    protected override Summary CreateSummary(DateOnly currentDate, decimal amount) =>
        new Summary(amount, DateRange.Create(currentDate, currentDate));
}
