using Domain.AggregatesModel.ReportAggregate.Reports;
using Domain.AggregatesModel.ReportAggregate.Reports.Builder;
using Domain.Entities.TransactionAggregate;
using Domain.Extensions;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
public class CreateDailyReportHandler : CreateReportHandler
{
    public IExpectsCurrency _reportBuilder;
    public CreateDailyReportHandler(IExpectsCurrency reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    public override Report CreateReport(IEnumerable<Transaction> transactions, Currency currency)
    {
        var report = _reportBuilder.WithCurrency(currency)
            .WithDailySummary(transactions)
            .Build();

        return report;
    }
}
