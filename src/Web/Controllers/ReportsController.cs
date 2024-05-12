﻿using Application.Abstractions.Messaging;
using Application.UseCases.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Authorize]
public class ReportsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GenerateReport(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetReportQuery(this.UserId, startDate, endDate);
        return HandleResult(await this.Mediator.Send(query, cancellationToken));
    }

    [HttpPost("save/json")]
    public async Task<IActionResult> SaveReportAsJson(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new SaveReportJsonQuery(this.UserId, startDate, endDate);
        return await SaveReport(query, cancellationToken);
    }

    [HttpPost("save/xml")]
    public async Task<IActionResult> SaveReportAsXml(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new SaveReportXmlQuery(this.UserId, startDate, endDate);
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