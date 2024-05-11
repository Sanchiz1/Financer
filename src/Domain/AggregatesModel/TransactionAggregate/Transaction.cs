using Domain.Abstractions;
using Domain.Common;
using Domain.Errors;
using Domain.ValueObjects;
using SharedKernel.Result;

namespace Domain.Entities.TransactionAggregate;
public class Transaction : Entity<Guid>, IAggregateRoot
{
    public Guid CategoryId { get; private set; }
    public TransactionCategory Category { get; private set; }
    public Money Amount { get; private set; }
    public Description Description { get; private set; }
    public DateTime OperationDate { get; private set; }

    public decimal RealAmount => Amount * (int)Category.OperationType;

    public Transaction() { }

    internal Transaction(
        Guid id,
        TransactionCategory category,
        Money amount,
        Description description,
        DateTime operationDate)
    {
        this.Id = id;
        CategoryId = category.Id;
        Category = category;
        Amount = amount;
        Description = description;
        OperationDate = operationDate;
    }

    public static Result<Transaction> Create(
        Guid fundId,
        TransactionCategory category,
        Money amount,
        Description description,
        DateTime operationDate)
    {
        if (amount <= amount.Currency.MinPositiveValue)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidAmount);
        }

        return new Transaction(
            Guid.NewGuid(),
            category,
            amount,
            description,
            operationDate);
    }
}