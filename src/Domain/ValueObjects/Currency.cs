using Domain.Common;
using System.Text.Json.Serialization;

namespace Domain.ValueObjects;
public class Currency : ValueObject
{
    public string Code { get; init; }

    private Currency() { this.Code = string.Empty; }
    private Currency(string code) => this.Code = code;

    internal static readonly Currency None = new(string.Empty);
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");
    public static readonly Currency Uah = new("UAH");

    public static readonly IReadOnlyCollection<Currency> All = [Usd, Eur, Uah];

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ??
            throw new ApplicationException("The currency is invalid.");
    }

    [JsonIgnore]
    public Money MinPositiveValue =>
        new(.01M, this);

    public Money Of(decimal amount) =>
        new(amount, this);

    public override string ToString() =>
        this.Code;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Code;
    }
}