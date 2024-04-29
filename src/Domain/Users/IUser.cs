using Domain.ValueObjects;

namespace Domain.Users;

public interface IUser
{
    Name FirstName { get; }
    Name LastName { get; }
    Currency PrefferedCurrency { get; }
}
