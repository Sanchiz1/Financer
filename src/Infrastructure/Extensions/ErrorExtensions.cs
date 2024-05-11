using Microsoft.AspNetCore.Identity;
using SharedKernel.Result;

namespace Infrastructure.Extensions;
public static class ErrorExtensions
{
    public static Error ToResultError(this IdentityError error)
    {
        return new Error(error.Code, error.Description);
    }
}
