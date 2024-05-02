using Domain.Common;

namespace Domain.ValueObjects;

public class DateRange : ValueObject
{
    private DateRange() { }

    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }

    public int LengthInDays => End.DayNumber - Start.DayNumber;

    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (start > end)
        {
            throw new ApplicationException("End date precedes start date.");
        }

        return new DateRange
        {
            Start = start,
            End = end,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}