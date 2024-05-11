using Domain.AggregatesModel.ReportAggregate.ReportBuilder;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;

namespace Domain.AggregatesModel.ReportAggregate.CreateReportHandler;
public class CreateWeeklyReportHandler : CreateReportHandler
{
    public IExpectsCurrency _reportBuilder;
    public CreateWeeklyReportHandler(IExpectsCurrency reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    public override Report CreateReport(IEnumerable<Transaction> transactions)
    {
        var dateRange = transactions.GetDateRange();
        if (dateRange.LengthInDays > 6)
        {
            var currency = GetCurrency(transactions);

            var report = _reportBuilder.WithCurrency(currency)
                .WithWeeklySummary(transactions)
                .Build();

            return report;
        }
        else
        {
            return base.CreateReport(transactions);
        }
    }
}
