using Application.Abstractions.Messaging;
using Domain.AggregatesModel.ReportAggregate;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.ValueObjects;
using SharedKernel.Result;

namespace Application.UseCases.Reports;
public sealed record GetReportQuery(Currency Currency, string UserId, DateOnly StartDate, DateOnly EndDate) : IQuery<Report>;

internal sealed class GetReportQueryHandler(
    ITransactionRepository transactionRepository,
    ReportMakerFacade reportMakerFacade)
    : IQueryHandler<GetReportQuery, Report>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ReportMakerFacade _reportMakerFacade = reportMakerFacade;

    public async Task<Result<Report>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        var dateRange = DateRange.Create(request.StartDate, request.EndDate);
        
        var transactions = await this._transactionRepository.GetInDateRangeAsync(
            request.UserId,
            dateRange,
            cancellationToken);

        return await this._reportMakerFacade.CreateReport(request.Currency, transactions);
    }
}