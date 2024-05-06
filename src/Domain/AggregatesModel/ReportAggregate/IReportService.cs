using Domain.AggregatesModel.ReportAggregate.Reports;
using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate;
public interface IReportService
{
    Report CreateReport(IEnumerable<Transaction> transactions, Currency currency);
}