using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions;
public static class DateExtensions
{
    public static DateOnly GetStartOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    public static DateOnly GetEndOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static DateOnly GetStartOfWeek(this DateOnly date)
    {
        int delta = DayOfWeek.Monday - date.DayOfWeek;
        return date.AddDays(delta);
    }

    public static DateOnly GetEndOfWeek(this DateOnly date)
    {
        return date.GetStartOfWeek().AddDays(6);
    }
    
    public static bool IsDate(this DateTime date, DateOnly otherDate)
    {
        return date.Year == otherDate.Year &&
            date.Month == otherDate.Month &&
            date.Day == otherDate.Day;
    }
}
