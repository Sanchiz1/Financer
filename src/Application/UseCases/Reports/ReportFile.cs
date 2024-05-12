namespace Application.UseCases.Reports;

public sealed record ReportFile(byte[] Bytes, string ContentType, string FileName);