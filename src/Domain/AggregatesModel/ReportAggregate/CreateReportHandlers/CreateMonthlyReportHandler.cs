using Domain.AggregatesModel.ReportAggregate.ReportBuilder;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.Extensions;

namespace Domain.AggregatesModel.ReportAggregate.CreateReportHandlers;
public class CreateMonthlyReportHandler : CreateReportHandler
{
    public IExpectsCurrency _reportBuilder;
    public CreateMonthlyReportHandler(IExpectsCurrency reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    public override Report CreateReport(IEnumerable<Transaction> transactions)
    {
        var dateRange = transactions.GetDateRange();
        if (dateRange.LengthInDays > 31)
        {
            var currency = GetCurrency(transactions);

            var report = _reportBuilder.WithCurrency(currency)
                .WithMonthlySummary(transactions)
                .Build();

            return report;
        }
        else
        {
            return base.CreateReport(transactions);
        }
    }
}
