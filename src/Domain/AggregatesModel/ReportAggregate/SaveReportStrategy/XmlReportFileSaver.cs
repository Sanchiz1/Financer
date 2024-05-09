using Domain.AggregatesModel.ReportAggregate.Reports;
using System.Xml.Serialization;

namespace Domain.AggregatesModel.ReportAggregate.SaveReportStrategy;
public sealed class XmlReportFileSaver : IReportFileSaver
{
    public byte[] SaveReport(Report report)
    {
        var xmlSerializer = new XmlSerializer(typeof(Report));

        using var stream = new MemoryStream();

        xmlSerializer.Serialize(stream, report);

        return stream.ToArray();
    }
}