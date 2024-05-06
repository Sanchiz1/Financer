using Domain.ValueObjects;

namespace Application.Common.Interfaces;
public interface IUser
{
    Name FirstName { get; }
    Name LastName { get; }
    Currency PrefferedCurrency { get; }
}
