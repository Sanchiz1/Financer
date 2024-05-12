using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Entities.TransactionAggregate;
using Domain.Errors;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Transactions;
public sealed record EditTransactionCommand(TransactionDto Transaction) : ICommand<Unit>;

internal sealed class EditTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IMapper mapper)
    : ICommandHandler<EditTransactionCommand, Unit>
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<Unit>> Handle(EditTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await this._transactionRepository.GetByIdAsync(request.Transaction.Id, cancellationToken);

        if (transaction is null)
        {
            return Result.Failure<Unit>(TransactionErrors.NotFound);
        }

        transaction = this._mapper.Map<Transaction>(request.Transaction);

        await this._transactionRepository.Update(transaction, cancellationToken);

        return Result.Success(Unit.Value);
    }
}