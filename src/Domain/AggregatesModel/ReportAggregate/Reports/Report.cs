using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.Reports;
public class Report
{
    public Currency Currency { get; set; }

    public List<Summary> Summaries { get; set; }

    private Report() { }

    public Report(Currency currency, List<Summary> summaries)
    {
        Currency = currency;
        Summaries = summaries;
    }
}