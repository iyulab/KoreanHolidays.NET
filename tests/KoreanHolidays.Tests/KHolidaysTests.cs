using KoreanHolidays;
using Xunit;

namespace KoreanHolidays.Tests;

public class KHolidaysTests
{
    [Fact]
    public void MinMaxYear_MatchData()
    {
        Assert.Equal(2018, KHolidays.MinYear);
        Assert.Equal(2026, KHolidays.MaxYear);
    }

    [Theory]
    [InlineData(2017, false)]
    [InlineData(2018, true)]
    [InlineData(2026, true)]
    [InlineData(2027, false)]
    public void IsSupportedYear_ChecksRange(int year, bool expected)
    {
        Assert.Equal(expected, KHolidays.IsSupportedYear(year));
    }

    [Fact]
    public void IsPublicHoliday_TrueOnHoliday()
    {
        Assert.True(KHolidays.IsPublicHoliday(new DateOnly(2026, 1, 1)));
    }

    [Fact]
    public void IsPublicHoliday_FalseOnWorkday()
    {
        Assert.False(KHolidays.IsPublicHoliday(new DateOnly(2026, 1, 2)));
    }

    [Fact]
    public void IsPublicHoliday_ThrowsOnUnsupportedYear()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => KHolidays.IsPublicHoliday(new DateOnly(2030, 1, 1)));
    }

    [Fact]
    public void GetHolidays_ByDate_ReturnsNames()
    {
        var names = KHolidays.GetHolidays(new DateOnly(2025, 5, 5)).Select(h => h.Name).ToArray();
        Assert.Equal(new[] { "어린이날", "부처님 오신 날" }, names);
    }

    [Fact]
    public void GetHolidays_ByDate_EmptyOnWorkday()
    {
        Assert.Empty(KHolidays.GetHolidays(new DateOnly(2026, 1, 2)));
    }

    [Fact]
    public void GetHolidays_ByDate_ThrowsOnUnsupportedYear()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => KHolidays.GetHolidays(new DateOnly(2030, 1, 1)));
    }

    [Fact]
    public void TryGetHolidays_TrueWithNamesOnHoliday()
    {
        bool ok = KHolidays.TryGetHolidays(new DateOnly(2026, 1, 1), out var holidays);
        Assert.True(ok);
        Assert.Equal("1월 1일", holidays.Single().Name);
    }

    [Fact]
    public void TryGetHolidays_FalseOnWorkday()
    {
        bool ok = KHolidays.TryGetHolidays(new DateOnly(2026, 1, 2), out var holidays);
        Assert.False(ok);
        Assert.Empty(holidays);
    }

    [Fact]
    public void TryGetHolidays_FalseOnUnsupportedYear_NoThrow()
    {
        bool ok = KHolidays.TryGetHolidays(new DateOnly(2030, 1, 1), out var holidays);
        Assert.False(ok);
        Assert.Empty(holidays);
    }

    [Fact]
    public void GetHolidays_ByYear_ReturnsSortedHolidays()
    {
        var list = KHolidays.GetHolidays(2026);
        Assert.Equal(22, list.Count);
        Assert.Equal(new DateOnly(2026, 1, 1), list[0].Date);
        var dates = list.Select(h => h.Date).ToArray();
        Assert.Equal(dates.OrderBy(d => d).ToArray(), dates);
    }

    [Fact]
    public void GetHolidays_ByYear_ThrowsOnUnsupportedYear()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => KHolidays.GetHolidays(2030));
    }

    [Fact]
    public void GetHolidaysInRange_ReturnsInclusiveRange()
    {
        var list = KHolidays.GetHolidaysInRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 3, 1));
        Assert.Contains(list, h => h.Date == new DateOnly(2026, 1, 1));   // 신정 (시작 포함)
        Assert.Contains(list, h => h.Date == new DateOnly(2026, 3, 1));   // 삼일절 (끝 포함)
        Assert.DoesNotContain(list, h => h.Date == new DateOnly(2026, 3, 2)); // 대체공휴일 (범위 밖)
    }

    [Fact]
    public void GetHolidaysInRange_SpansYears()
    {
        var list = KHolidays.GetHolidaysInRange(new DateOnly(2025, 12, 25), new DateOnly(2026, 1, 1));
        Assert.Contains(list, h => h.Date == new DateOnly(2025, 12, 25)); // 기독탄신일
        Assert.Contains(list, h => h.Date == new DateOnly(2026, 1, 1));   // 신정
    }

    [Fact]
    public void GetHolidaysInRange_ThrowsWhenStartAfterEnd()
    {
        Assert.Throws<ArgumentException>(
            () => KHolidays.GetHolidaysInRange(new DateOnly(2026, 3, 1), new DateOnly(2026, 1, 1)));
    }

    [Fact]
    public void GetHolidaysInRange_ThrowsOnUnsupportedYear()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => KHolidays.GetHolidaysInRange(new DateOnly(2030, 1, 1), new DateOnly(2030, 12, 31)));
    }

    [Fact]
    public void NewlyDesignated2026Holidays_ArePublicHolidays()
    {
        // 2025년 공휴일법 개정으로 2026년부터 노동절(5/1)·제헌절(7/17)이 법정공휴일.
        Assert.True(KHolidays.IsPublicHoliday(new DateOnly(2026, 5, 1)));
        Assert.True(KHolidays.IsPublicHoliday(new DateOnly(2026, 7, 17)));
        Assert.Equal("노동절", KHolidays.GetHolidays(new DateOnly(2026, 5, 1)).Single().Name);
        Assert.Equal("제헌절", KHolidays.GetHolidays(new DateOnly(2026, 7, 17)).Single().Name);
    }
}
