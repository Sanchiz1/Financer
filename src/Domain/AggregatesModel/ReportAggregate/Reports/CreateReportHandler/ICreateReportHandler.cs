using Domain.AggregatesModel.TransactionAggregate;

namespace Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
public interface ICreateReportHandler
{
    Report CreateReport(IEnumerable<Transaction> transactions);

    ICreateReportHandler SetNext(ICreateReportHandler handler);
}
