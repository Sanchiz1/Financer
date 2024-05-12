using SharedKernel.Result;

namespace Domain.Errors;
public class TransactionErrors
{
    public static Error NotFound = new(
        "Transaction.NotFound",
        "The transaction with the specified identifier was not found");

    public static Error InvalidAmount = new(
        "Transaction.InvalidAmount",
        "The amount specified for the transaction is invalid");

    public static Error InvalidUser = new(
        "Transaction.InvalidUser",
        "The provided user credentials are invalid");

    public static Error InvalidCategory = new(
        "Transaction.InvalidCategory",
        "The provided category is invalid or does not exist");
}