using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.ValueObjects;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record GetTransactionsInDateRangeQuery(
    string UserId,
    DateOnly StartDate,
    DateOnly EndDate) : IQuery<IReadOnlyList<TransactionDto>>;

internal sealed class GetTransactionsInDateRangeQueryHandler(ITransactionRepository transactionRepository, IMapper mapper)
    : IQueryHandler<GetTransactionsInDateRangeQuery, IReadOnlyList<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IReadOnlyList<TransactionDto>>> Handle(GetTransactionsInDateRangeQuery request, CancellationToken cancellationToken)
    {
        var dateRange = DateRange.Create(request.StartDate, request.EndDate);

        var transactions = await this._transactionRepository.GetInDateRangeAsync(request.UserId, dateRange, cancellationToken);

        return this._mapper.Map<List<TransactionDto>>(transactions);
    }
}