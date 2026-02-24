namespace SideEffect.Extensions;

/// <summary>
/// A set of extension methods for date time object.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Returns first day of the week.
    /// </summary>
    /// <param name="date">Date for which first day of the week should be found.</param>
    /// <param name="startDayOfWeek">First day of the week.</param>
    /// <returns></returns>
    public static DateOnly FirstDayOfWeek(this DateTime date, DayOfWeek startDayOfWeek)
    {
        if (startDayOfWeek != DayOfWeek.Monday && startDayOfWeek != DayOfWeek.Sunday)
        {
            throw new NotSupportedException($"'{startDayOfWeek}' is not supported as start day of the week.");
        }

        int diff = (7 + (date.DayOfWeek - startDayOfWeek)) % 7;
        return DateOnly.FromDateTime(date.AddDays(-1 * diff));
    }

    /// <summary>
    /// Returns last day of the week.
    /// </summary>
    /// <param name="date">Date for which last day of the week should be found.</param>
    /// <param name="startDayOfWeek">First day of the week.</param>
    /// <returns></returns>
    public static DateOnly LastDayOfWeek(this DateTime date, DayOfWeek startDayOfWeek)
    {
        int diff = (7 + (date.DayOfWeek - startDayOfWeek)) % 7;
        return DateOnly.FromDateTime(date.AddDays(-1 * diff + 6));
    }

    /// <summary>
    /// Returns first day of the month.
    /// </summary>
    /// <param name="date">Date for which first day of the month should be found.</param>
    /// <returns></returns>
    public static DateOnly FirstDayOfMonth(this DateTime date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    /// <summary>
    /// Returns last day of the month.
    /// </summary>
    /// <param name="date">Date for which last day of the month should be found.</param>
    /// <returns></returns>
    public static DateOnly LastDayOfMonth(this DateTime date)
    {
        return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }
}
