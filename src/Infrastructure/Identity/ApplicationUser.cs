using Application.Common.Interfaces;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;
public class ApplicationUser : IdentityUser<Guid>, IUser
{
    public Name FirstName { get; set; }

    public Name LastName { get; set; }

    public Currency PrefferedCurrency { get; set; }
}