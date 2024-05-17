﻿using Application.Abstractions.Messaging;
using Domain.AggregatesModel.ReportAggregate;
using Domain.AggregatesModel.ReportAggregate.SaveReportStrategy;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Result;

namespace Application.UseCases.Reports;
public sealed record SaveReportXmlQuery(Currency Currency, string UserId, DateOnly StartDate, DateOnly EndDate) : IQuery<ReportFile>;

internal sealed class SaveReportXmlQueryHandler(
    ITransactionRepository transactionRepository,
    [FromKeyedServices("save-report-xml")] IReportFileSaver jsonSaver,
    ReportMakerFacade reportMakerFacade)
    : IQueryHandler<SaveReportXmlQuery, ReportFile>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IReportFileSaver _jsonSaver = jsonSaver;
    private readonly ReportMakerFacade _reportMakerFacade = reportMakerFacade;

    public async Task<Result<ReportFile>> Handle(SaveReportXmlQuery request, CancellationToken cancellationToken)
    {
        var dateRange = DateRange.Create(request.StartDate, request.EndDate);
        var transactions = await this._transactionRepository.GetInDateRangeAsync(
            request.UserId, 
            dateRange, 
            cancellationToken);

        var report = await this._reportMakerFacade.CreateReport(request.Currency, transactions);

        byte[] xmlBytes = this._jsonSaver.SaveReport(report.Value);
        string filename = $"report_{DateTime.Now:yyyyMMddHHmmss}.xml";

        return new ReportFile(xmlBytes, "application/xml", filename);
    }
}