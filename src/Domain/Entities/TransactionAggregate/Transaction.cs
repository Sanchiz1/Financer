﻿using Domain.Abstractions;
using SharedKernel.Result;
using Domain.Errors;
using Domain.ValueObjects;

namespace Domain.Entities.TransactionAggregate;

public class Transaction : BaseEntity<Guid>
{
    public Guid FundId { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionCategory Category { get; private set; }
    public Money Amount { get; private set; }
    public Description Description { get; private set; }
    public DateTime OperationDate { get; private set; }


    internal Transaction(
        Guid id,
        Guid fundId, 
        TransactionCategory category,
        Money amount,
        Description description,
        DateTime operationDate)
    {
        this.Id = id;
        FundId = fundId;
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
            fundId,
            category,
            amount,
            description,
            operationDate);
    }
}