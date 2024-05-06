using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.Reports;
public class Summary
{
    public decimal Amount {  get; private set; }
    public DateRange DateRange {  get; private set; }

    public Summary(decimal amount, DateRange dateRange)
    {
        Amount = amount;
        DateRange = dateRange;
    }
}
