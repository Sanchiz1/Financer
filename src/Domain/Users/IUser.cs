using Domain.Shared;
using Domain.Currencies;

namespace Domain.Users
{
    public interface IUser
    {
        Name FirstName { get; }
        Name LastName { get; }
        Currency PrefferedCurrency { get; }
    }
}
