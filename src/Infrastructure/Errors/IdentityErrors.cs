using SharedKernel.Result;

namespace Infrastructure.Errors;
public static class IdentityErrors
{
    public static Error UserNotFound = new(
        "404",
        "User not found");
    public static Error InvalidPasswordOrUsername = new (
        "400",
        "Invalid password or username");
}
