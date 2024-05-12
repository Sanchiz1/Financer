using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record GetTransactionsQuery(string UserId) : IQuery<IReadOnlyList<TransactionDto>>;

internal sealed class GetTransactionsQueryHandler(ITransactionRepository transactionRepository, IMapper mapper) 
    : IQueryHandler<GetTransactionsQuery, IReadOnlyList<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IReadOnlyList<TransactionDto>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await this._transactionRepository.GetAllAsync(request.UserId, cancellationToken);

        return this._mapper.Map<List<TransactionDto>>(transactions);
    }
}