using Domain.Entities.TransactionAggregate;
namespace Domain.AggregatesModel.ReportAggregate;
public interface IReportService
{
    Report CreateReport(IEnumerable<Transaction> transactions);
}