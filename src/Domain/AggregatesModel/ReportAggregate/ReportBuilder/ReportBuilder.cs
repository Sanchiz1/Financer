using Domain.AggregatesModel.ReportAggregate.ReportBuilder.SummaryMappers;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder;
public class ReportBuilder : IReportBuilder, IExpectsCurrency, IExpectsSummary
{
    private Currency Currency { get; set; } = Currency.None;
    private List<Summary> Summaries { get; set; } = [];

    public IExpectsSummary WithCurrency(Currency currency)
    {
        Currency = currency;

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
        SummaryMapper summaryMapper = new DailySummaryMapper();

        Summaries = summaryMapper.MapToSummaries(transactions).ToList();

        return this;
    }

    public IReportBuilder WithWeeklySummary(IEnumerable<Transaction> transactions)
    {
        SummaryMapper summaryMapper = new WeeklySummaryMapper();

        Summaries = summaryMapper.MapToSummaries(transactions).ToList();

        return this;
    }

    public IReportBuilder WithMonthlySummary(IEnumerable<Transaction> transactions)
    {
        SummaryMapper summaryMapper = new MonthlySummaryMapper();

        Summaries = summaryMapper.MapToSummaries(transactions).ToList();

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
