using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.Reports;
public class Report
{
    public Currency Currency { get; private set; }

    public List<Summary> Summaries { get; private set; }

    public Report(Currency currency, List<Summary> summaries)
    {
        Currency = currency;
        Summaries = summaries;
    }
}
