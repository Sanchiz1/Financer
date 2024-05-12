using Domain.AggregatesModel.TransactionAggregate;

namespace Domain.AggregatesModel.ReportAggregate.CreateReportHandlers;
public interface ICreateReportHandler
{
    Report CreateReport(IEnumerable<Transaction> transactions);

    ICreateReportHandler SetNext(ICreateReportHandler handler);
}
