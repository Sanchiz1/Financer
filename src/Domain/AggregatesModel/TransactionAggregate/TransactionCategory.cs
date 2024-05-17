using Domain.Common;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.TransactionAggregate;
public class TransactionCategory : Entity<Guid>
{
    public Guid UserId { get; set; }
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public OperationType OperationType { get; private set; }

    public TransactionCategory(Guid userId, Name name, Description description, OperationType operationType)
    {
        UserId = userId;
        this.Name = name;
        this.Description = description;
        this.OperationType = operationType;
    }
}