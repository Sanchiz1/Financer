using Application.Abstractions.Messaging;
using Domain.AggregatesModel.ReportAggregate;
using Domain.AggregatesModel.ReportAggregate.CreateReportHandlers;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.ValueObjects;
using SharedKernel.Result;

namespace Application.UseCases.Reports;
public sealed record GetReportQuery(string UserId, DateOnly StartDate, DateOnly EndDate) : IQuery<Report>;

internal sealed class GetReportQueryHandler(
    ITransactionRepository transactionRepository,
    ICreateReportHandler createReportHandler)
    : IQueryHandler<GetReportQuery, Report>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ICreateReportHandler _createReportHandler = createReportHandler;

    public async Task<Result<Report>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        var dateRange = DateRange.Create(request.StartDate, request.EndDate);
        var transactions = await this._transactionRepository.GetInDateRangeAsync(
            request.UserId,
            dateRange,
            cancellationToken);

        return this._createReportHandler.CreateReport(transactions);
    }
}