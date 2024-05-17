using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Application.UseCases.Reports;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Authorize]
public class ReportsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GenerateReport(
        GenerateReportDto generateReportDto,
        CancellationToken cancellationToken)
    {
        var query = new GetReportQuery(
            Currency.FromCode(generateReportDto.Currency), 
            this.UserId, 
            generateReportDto.DateRange.Start,
            generateReportDto.DateRange.End);

        return HandleResult(await this.Mediator.Send(query, cancellationToken));
    }

    [HttpPost("save/json")]
    public async Task<IActionResult> SaveReportAsJson(
        GenerateReportDto generateReportDto,
        CancellationToken cancellationToken)
    {
        var query = new SaveReportJsonQuery(
            Currency.FromCode(generateReportDto.Currency), 
            this.UserId, 
            generateReportDto.DateRange.Start, 
            generateReportDto.DateRange.End);

        return await SaveReport(query, cancellationToken);
    }

    [HttpPost("save/xml")]
    public async Task<IActionResult> SaveReportAsXml(
        GenerateReportDto generateReportDto,
        CancellationToken cancellationToken)
    {
        var query = new SaveReportXmlQuery(
            Currency.FromCode(generateReportDto.Currency),
            this.UserId,
            generateReportDto.DateRange.Start,
            generateReportDto.DateRange.End);

        return await SaveReport(query, cancellationToken);
    }

    private async Task<IActionResult> SaveReport<TQuery>(
        TQuery query,
        CancellationToken cancellationToken)
        where TQuery : IQuery<ReportFile>
    {
        var result = await this.Mediator.Send(query, cancellationToken);

        if (result.IsSuccess && result.Value != null)
        {
            var reportFile = result.Value;
            return File(reportFile.Bytes, reportFile.ContentType, reportFile.FileName);
        }
        else
        {
            return HandleResult(result);
        }
    }
}