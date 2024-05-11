using Domain.AggregatesModel.TransactionAggregate;

namespace Domain.AggregatesModel.ReportAggregate.CreateReportHandler;
public interface ICreateReportHandler
{
    Report CreateReport(IEnumerable<Transaction> transactions);

    ICreateReportHandler SetNext(ICreateReportHandler handler);
}
