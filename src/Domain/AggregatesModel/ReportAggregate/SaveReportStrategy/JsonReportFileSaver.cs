using Domain.AggregatesModel.ReportAggregate.Reports;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.AggregatesModel.ReportAggregate.SaveReportStrategy;
public sealed class JsonReportFileSaver : IReportFileSaver
{
    public byte[] SaveReport(Report report)
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(report, options);

        return System.Text.Encoding.UTF8.GetBytes(json);
    }
}