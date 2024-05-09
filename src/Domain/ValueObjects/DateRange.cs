using Domain.Common;
using System.Xml.Serialization;

namespace Domain.ValueObjects;
public class DateRange : ValueObject
{
    private DateRange() { }

    [XmlElement("StartDate")]
    public string StartDate
    {
        get => Start.ToString("yyyy-MM-dd");
        set => Start = DateOnly.Parse(value);
    }

    [XmlIgnore]
    public DateOnly Start { get; private set; }

    [XmlElement("EndDate")]
    public string EndDate
    {
        get => End.ToString("yyyy-MM-dd");
        set => End = DateOnly.Parse(value);
    }

    [XmlIgnore]
    public DateOnly End { get; private set; }

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