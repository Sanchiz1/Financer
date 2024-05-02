using Domain.Abstractions;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities.TransactionAggregate;

public class TransactionCategory : Entity<Guid>
{
    public Guid UserId { get; set; }
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public OperationType OperationType { get; private set; }

    public TransactionCategory(Guid userId, Name name, Description description, OperationType operationType)
    {
        UserId = userId;
        Name = name;
        Description = description;
        OperationType = operationType;
    }
}