using SharedKernel.Result;

namespace Domain.Errors;
public static class CategoryErrors
{
    public static Error NotFound = new(
        "Category.NotFound",
        "The category with the specified identifier was not found");

    public static Error InvalidType = new(
        "Category.InvalidType",
        "The provided operation type is invalid for the category");

    public static Error InvalidName = new(
        "Category.InvalidName",
        "The name provided for the category is invalid");

    public static Error InvalidUser = new(
        "Transaction.InvalidUser",
        "The provided user credentials are invalid");
}