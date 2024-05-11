using Domain.ValueObjects;
using System.Runtime.CompilerServices;

namespace Domain.AggregatesModel.ReportAggregate.Reports;
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