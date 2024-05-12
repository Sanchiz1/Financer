using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder.SummaryMappers;
public class MonthlySummaryMapper : SummaryMapper
{
    protected override DateOnly GetStartDate(DateOnly date)
    {
        return date.GetStartOfMonth();
    }
    protected override DateOnly NextDate(DateOnly currentDate)
    {
        return currentDate.AddMonths(1);
    }

    protected override decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions)
    {
        var startOfMonthDateTime = currentDate.GetStartOfMonth().ToDateTime(TimeOnly.MinValue);
        var endOfMonthDateTime = currentDate.GetEndOfMonth().ToDateTime(TimeOnly.MaxValue);

        return transactions.Where(t =>
        t.OperationDate.Date >= startOfMonthDateTime &&
        t.OperationDate.Date <= endOfMonthDateTime)
            .Sum(o => o.RealAmount);
    }

    protected override Summary CreateSummary(DateOnly currentDate, decimal amount)
    {
        return new Summary(amount,
                DateRange.Create(currentDate.GetStartOfMonth(),
                currentDate.GetEndOfMonth())
                );
    }
}
