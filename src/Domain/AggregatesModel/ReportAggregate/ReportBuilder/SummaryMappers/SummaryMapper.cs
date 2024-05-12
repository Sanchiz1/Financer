using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder.SummaryMappers;
public abstract class SummaryMapper
{
    public IEnumerable<Summary> MapToSummaries(IEnumerable<Transaction> transactions)
    {
        var summaries = new List<Summary>();

        var dateRange = transactions.GetDateRange();

        var currentDate = GetStartDate(dateRange.Start);

        while (currentDate <= dateRange.End)
        {
            decimal totalAmount = GetTotalAmount(currentDate, transactions);

            var summary = CreateSummary(currentDate, totalAmount);

            summaries.Add(summary);

            currentDate = NextDate(currentDate);
        }

        return summaries;
    }

    protected abstract DateOnly GetStartDate(DateOnly date);
    protected abstract DateOnly NextDate(DateOnly currentDate);
    protected abstract decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions);
    protected abstract Summary CreateSummary(DateOnly currentDate, decimal amount);
}
