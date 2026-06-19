using KoreanHolidays;
using Xunit;

namespace KoreanHolidays.Tests;

public class HolidayTests
{
    [Fact]
    public void Holiday_StoresAllProperties()
    {
        var h = new Holiday
        {
            Date = new DateOnly(2026, 8, 17),
            Name = "대체공휴일(광복절)",
            IsSubstitute = true,
            SubstituteFor = "광복절",
        };

        Assert.Equal(new DateOnly(2026, 8, 17), h.Date);
        Assert.Equal("대체공휴일(광복절)", h.Name);
        Assert.True(h.IsSubstitute);
        Assert.Equal("광복절", h.SubstituteFor);
    }

    [Fact]
    public void Holiday_DefaultsForRegularHoliday()
    {
        var h = new Holiday { Date = new DateOnly(2026, 5, 5), Name = "어린이날" };

        Assert.False(h.IsSubstitute);
        Assert.Null(h.SubstituteFor);
    }

    [Fact]
    public void Holiday_ValueEquality()
    {
        var a = new Holiday { Date = new DateOnly(2026, 1, 1), Name = "1월 1일" };
        var b = new Holiday { Date = new DateOnly(2026, 1, 1), Name = "1월 1일" };

        Assert.Equal(a, b);
    }
}
