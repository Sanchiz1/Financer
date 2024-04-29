using SharedKernel.Result;

namespace Domain.Errors;

public static class CurrencyErrors
{
    public static Error NotFound = new(
        "Currency.NotFound",
        "The currency with the specified code was not found");

    public static Error ConversionError = new(
        "Currency.ConversionError",
        "An error occurred during currency conversion");
}