using Domain.AggregatesModel.ReportAggregate.ReportBuilder;
using Domain.AggregatesModel.TransactionAggregate;

namespace Domain.AggregatesModel.ReportAggregate.CreateReportHandlers;
public class CreateDailyReportHandler : CreateReportHandler
{
    public IExpectsCurrency _reportBuilder;
    public CreateDailyReportHandler(IExpectsCurrency reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    public override Report CreateReport(IEnumerable<Transaction> transactions)
    {
        var currency = GetCurrency(transactions);

        var report = _reportBuilder.WithCurrency(currency)
            .WithDailySummary(transactions)
            .Build();

        return report;
    }
}
