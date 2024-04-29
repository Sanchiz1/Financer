using SharedKernel.Result;

namespace Domain.Errors;

public static class FundErrors
{
    public static Error NotFound = new(
        "Fund.NotFound",
        "The fund with the specified identifier was not found");

    public static Error InvalidCurrency = new(
        "Fund.InvalidCurrency",
        "The provided currency is not supported");

    public static Error InvalidName = new(
        "Fund.InvalidName",
        "The name provided for the fund is invalid");

    public static Error InvalidAmount = new(
        "Fund.InvalidAmount",
        "The amount specified for the fund operation is invalid");
}
