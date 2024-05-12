using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Errors;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record GetTransactionByIdQuery(string UserId, Guid TransactionId) : IQuery<TransactionDto>;

internal sealed class GetTransactionByIdQueryHandler(
    ITransactionRepository transactionRepository,
    IMapper mapper,
    ITransactionAuthorizationService transactionAuthorizationService)
    : IQueryHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ITransactionAuthorizationService _transactionAuthorizationService = transactionAuthorizationService;

    public async Task<Result<TransactionDto>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        bool isAccessible = await this._transactionAuthorizationService.IsTransactionAccessible(
            request.UserId,
            request.TransactionId,
            cancellationToken);

        if (!isAccessible)
        {
            return Result.Failure<TransactionDto>(TransactionErrors.InvalidUser);
        }

        var transaction = await this._transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is null)
        {
            return Result.Failure<TransactionDto>(TransactionErrors.NotFound);
        }

        return this._mapper.Map<TransactionDto>(transaction);
    }
}