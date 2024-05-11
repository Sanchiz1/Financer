using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate;
public class Summary
{
    public decimal Amount { get; set; }
    public DateRange DateRange { get; set; }

    private Summary() { }

    public Summary(decimal amount, DateRange dateRange)
    {
        Amount = amount;
        DateRange = dateRange;
    }
}