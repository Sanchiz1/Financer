using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder.SummaryMappers;
public class DailySummaryMapper : SummaryMapper
{
    protected override DateOnly GetStartDate(DateOnly date)
    {
        return date;
    }
    protected override DateOnly NextDate(DateOnly currentDate)
    {
        return currentDate.AddDays(1);
    }

    protected override decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions)
    {
        return transactions.Where(t =>
        t.OperationDate.IsDate(currentDate))
            .Sum(o => o.RealAmount);
    }

    protected override Summary CreateSummary(DateOnly currentDate, decimal amount)
    {
        return new Summary(amount,
            DateRange.Create(currentDate, currentDate)
            );
    }
}
