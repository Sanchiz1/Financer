using Domain.Entities.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.Reports.Builder;
public class ReportBuilder : IReportBuilder, IExpectsCurrency, IExpectsSummary
{
    private Currency Currency { get; set; } = Currency.None;
    private List<Summary> Summaries { get; set; } = [];

    public IExpectsSummary WithCurrency(Currency currency)
    {
        this.Currency = currency;

        return this;
    }

    public IReportBuilder WithSummary(IEnumerable<Transaction> transactions)
    {
        var dateRange = transactions.GetDateRange();

        var amount = transactions.Sum(t => t.RealAmount);

        var summary = new Summary(amount, dateRange);

        Summaries.Add(summary);

        return this;
    }

    public IReportBuilder WithDailySummary(IEnumerable<Transaction> transactions)
    {
        var dateRange = transactions.GetDateRange();

        var currentDate = dateRange.Start;

        while (currentDate <= dateRange.End)
        {
            decimal totalAmount = transactions.Where(t =>
            t.OperationDate.IsDate(currentDate))
                .Sum(o => o.RealAmount);

            var summary = new Summary(totalAmount,
                DateRange.Create(currentDate, currentDate)
                );

            Summaries.Add(summary);

            currentDate = currentDate.AddDays(1);
        }

        return this;
    }

    public IReportBuilder WithWeeklySummary(IEnumerable<Transaction> transactions)
    {
        var dateRange = transactions.GetDateRange();

        var currentDate = dateRange.Start.GetStartOfWeek();

        while (currentDate <= dateRange.End)
        {
            var startOfWeekDateTime = currentDate.GetStartOfWeek().ToDateTime(TimeOnly.MinValue);
            var endOfWeekDateTime = currentDate.GetEndOfWeek().ToDateTime(TimeOnly.MaxValue);

            decimal totalAmount = transactions.Where(t =>
            t.OperationDate.Date >= startOfWeekDateTime &&
            t.OperationDate.Date <= endOfWeekDateTime)
                .Sum(o => o.RealAmount);

            var summary = new Summary(totalAmount,
                DateRange.Create(currentDate.GetStartOfWeek(), currentDate.GetEndOfWeek())
                );

            Summaries.Add(summary);

            currentDate = currentDate.AddDays(7);
        }

        return this;
    }

    public IReportBuilder WithMonthlySummary(IEnumerable<Transaction> transactions)
    {
        Summaries = [];

        var dateRange = transactions.GetDateRange();

        var currentDate = dateRange.Start.GetStartOfMonth();

        while (currentDate <= dateRange.End)
        {
            var startOfMonthDateTime = currentDate.GetStartOfMonth().ToDateTime(TimeOnly.MinValue);
            var endOfMonthDateTime = currentDate.GetEndOfMonth().ToDateTime(TimeOnly.MaxValue);

            decimal totalAmount = transactions.Where(t =>
            t.OperationDate.Date >= startOfMonthDateTime &&
            t.OperationDate.Date <= endOfMonthDateTime)
                .Sum(o => o.RealAmount);

            var summary = new Summary(totalAmount,
                DateRange.Create(currentDate.GetStartOfMonth(), currentDate.GetEndOfMonth())
                );

            Summaries.Add(summary);

            currentDate = currentDate.AddMonths(1);
        }

        return this;
    }

    public Report Build()
    {
        var report = new Report(Currency, Summaries);

        Currency = Currency.None;
        Summaries = [];

        return report;
    }
}
