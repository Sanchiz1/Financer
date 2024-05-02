using Domain.Currencies;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;
public class ApplicationUser : IdentityUser<Guid>, IUser
{
    public Name FirstName { get; private set; }

    public Name LastName { get; private set; }

    public Currency PrefferedCurrency { get; private set; }
}