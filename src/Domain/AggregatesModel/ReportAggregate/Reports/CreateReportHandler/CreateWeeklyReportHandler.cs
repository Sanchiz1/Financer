using Domain.AggregatesModel.ReportAggregate.Reports;
using Domain.AggregatesModel.ReportAggregate.Reports.Builder;
using Domain.Entities.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
public class CreateWeeklyReportHandler : CreateReportHandler
{
    public IExpectsCurrency _reportBuilder;
    public CreateWeeklyReportHandler(IExpectsCurrency reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    public override Report CreateReport(IEnumerable<Transaction> transactions, Currency currency)
    {
        var dateRange = transactions.GetDateRange();
        if (dateRange.LengthInDays > 6)
        {
            var report = _reportBuilder.WithCurrency(currency)
                .WithWeeklySummary(transactions)
                .Build();

            return report;
        }
        else
        {
            return base.CreateReport(transactions, currency);
        }
    }
}
