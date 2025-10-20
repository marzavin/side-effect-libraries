using NUnit.Framework;

namespace SideEffect.Extensions.Tests;

[TestFixture]
public class DateTimeExtensionsTests
{

    [TestCase(2025, 10, 20, DateTimeKind.Utc, DayOfWeek.Monday, 20)]
    [TestCase(2025, 10, 22, DateTimeKind.Utc, DayOfWeek.Monday, 20)]
    [TestCase(2025, 10, 25, DateTimeKind.Utc, DayOfWeek.Monday, 20)]
    [TestCase(2025, 10, 26, DateTimeKind.Utc, DayOfWeek.Monday, 20)]
    [TestCase(2025, 10, 20, DateTimeKind.Utc, DayOfWeek.Sunday, 19)]
    [TestCase(2025, 10, 22, DateTimeKind.Utc, DayOfWeek.Sunday, 19)]
    [TestCase(2025, 10, 25, DateTimeKind.Utc, DayOfWeek.Sunday, 19)]
    [TestCase(2025, 10, 26, DateTimeKind.Utc, DayOfWeek.Sunday, 26)]
    public void FirstDayOfWeekTest(int year, int month, int day, DateTimeKind kind, DayOfWeek dayOfWeek, int expectedDay)
    {
        var dateTime = new DateTime(year, month, day, 0, 0, 0, kind);
        var firstDayOfWeek = dateTime.FirstDayOfWeek(dayOfWeek);

        Assert.That(firstDayOfWeek.Day, Is.EqualTo(expectedDay));
    }

    [TestCase(2025, 10, 13, DateTimeKind.Utc, DayOfWeek.Monday, 19)]
    [TestCase(2025, 10, 15, DateTimeKind.Utc, DayOfWeek.Monday, 19)]
    [TestCase(2025, 10, 18, DateTimeKind.Utc, DayOfWeek.Monday, 19)]
    [TestCase(2025, 10, 19, DateTimeKind.Utc, DayOfWeek.Monday, 19)]
    [TestCase(2025, 10, 13, DateTimeKind.Utc, DayOfWeek.Sunday, 18)]
    [TestCase(2025, 10, 15, DateTimeKind.Utc, DayOfWeek.Sunday, 18)]
    [TestCase(2025, 10, 18, DateTimeKind.Utc, DayOfWeek.Sunday, 18)]
    [TestCase(2025, 10, 19, DateTimeKind.Utc, DayOfWeek.Sunday, 25)]
    public void LastDayOfWeekTest(int year, int month, int day, DateTimeKind kind, DayOfWeek dayOfWeek, int expectedDay)
    {
        var dateTime = new DateTime(year, month, day, 0, 0, 0, kind);
        var firstDayOfWeek = dateTime.LastDayOfWeek(dayOfWeek);

        Assert.That(firstDayOfWeek.Day, Is.EqualTo(expectedDay));
    }
}
