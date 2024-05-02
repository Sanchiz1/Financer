using Domain.Abstractions;
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities.FundAggregate;

public sealed class Fund : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public Currency Currency { get; private set; }
    public Name Name { get; private set; }
    public Description Description { get; private set; }

    public Fund(Guid userId, Currency currency, Name name, Description description)
    {
        UserId = userId;
        Currency = currency;
        Name = name;
        Description = description;
    }
}
