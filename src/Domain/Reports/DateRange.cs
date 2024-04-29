namespace Domain.Reports;

public record DateRange
{
    private DateRange() { }

    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }

    public int LengthInDays => this.End.DayNumber - this.Start.DayNumber;

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
}