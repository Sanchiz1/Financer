using Domain.AggregatesModel.ReportAggregate.Reports;
using Domain.Entities.TransactionAggregate;
using Domain.Extensions;

namespace Domain.AggregatesModel.ReportAggregate;
public class CreateMonthlyReportHandler : CreateReportHandler
{
    public override Report CreateReport(IEnumerable<Transaction> transactions)
    {
        var dateRange = transactions.GetDateRange();
        if(dateRange.LengthInDays > 31)
        {
            throw new NotImplementedException();
           /* var start = dateRange.Start.ToDateTime(new TimeOnly());

            transactions.Where(t => t.OperationDate >= start && t.OperationDate <= start.GetEndOfMonthDateTime());*/
        }
        else
        {
            return base.CreateReport(transactions);
        }
    }
}
