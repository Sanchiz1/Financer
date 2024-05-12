using Application.Common.Dtos;
using Application.UseCases.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Infrastructure;

namespace Web.Controllers.Transactions
{
    [Authorize]
    public class TransactionsController : BaseApiController
    {

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTransactionByIdQuery(GetUserId(), id);
            return HandleResult(await this.Mediator.Send(query, cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
        {
            var query = new GetTransactionsQuery(GetUserId()!);
            return HandleResult(await this.Mediator.Send(query, cancellationToken));
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetTransactions(
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken)
        {
            var query = new GetTransactionsInDateRangeQuery(GetUserId(), startDate, endDate);
            return HandleResult(await this.Mediator.Send(query, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] TransactionDto transaction,
            CancellationToken cancellationToken)
        {
            var query = new CreateTransactionCommand(transaction);
            return HandleResult(await this.Mediator.Send(query, cancellationToken));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTransaction(
            Guid id,
            [FromBody] TransactionDto transactionDto,
            CancellationToken cancellationToken)
        {
            transactionDto = transactionDto with { Id = id };
            var query = new EditTransactionCommand(transactionDto);
            return HandleResult(await this.Mediator.Send(query, cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id, CancellationToken cancellationToken)
        {
            var query = new DeleteTransactionCommand(GetUserId(), id);
            return HandleResult(await this.Mediator.Send(query, cancellationToken));
        }

        private string GetUserId()
        {
            return this.User.GetUserId().ToString();
        }
    }
}