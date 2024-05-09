using Domain.AggregatesModel.ReportAggregate.Reports;

namespace Domain.AggregatesModel.ReportAggregate.SaveReportStrategy;
public interface IReportFileSaver
{
    byte[] SaveReport(Report report);
}