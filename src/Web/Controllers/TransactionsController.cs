using Application.Common.Dtos;
using Application.UseCases.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Authorize]
public class TransactionsController : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionByIdQuery(this.UserId, id);
        return HandleResult(await this.Mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
    {
        var query = new GetTransactionsQuery(this.UserId);
        return HandleResult(await this.Mediator.Send(query, cancellationToken));
    }

    [HttpGet("range")]
    public async Task<IActionResult> GetTransactions(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionsInDateRangeQuery(this.UserId, startDate, endDate);
        return HandleResult(await this.Mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(
        [FromBody] TransactionDto transaction,
        CancellationToken cancellationToken)
    {
        var command = new CreateTransactionCommand(transaction);
        return HandleResult(await this.Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditTransaction(
        Guid id,
        [FromBody] TransactionDto transactionDto,
        CancellationToken cancellationToken)
    {
        transactionDto = transactionDto with { Id = id };
        var command = new EditTransactionCommand(this.UserId, transactionDto);
        return HandleResult(await this.Mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var command = new DeleteTransactionCommand(this.UserId, id);
        return HandleResult(await this.Mediator.Send(command, cancellationToken));
    }
}