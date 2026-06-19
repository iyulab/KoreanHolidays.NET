using System.Collections.Immutable;
using KoreanHolidays;
using Xunit;

namespace KoreanHolidays.Tests;

public class DataIntegrityTests
{
    [Fact]
    public void Store_HasExpectedTotalEntries()
    {
        Assert.Equal(168, HolidayStore.AllSorted.Length);
    }

    [Fact]
    public void Store_HasExpectedSubstituteCount()
    {
        int subs = HolidayStore.AllSorted.Count(h => h.IsSubstitute);
        Assert.Equal(20, subs);
    }

    [Fact]
    public void Store_YearRangeIs2018To2026()
    {
        Assert.Equal(2018, HolidayStore.MinYear);
        Assert.Equal(2026, HolidayStore.MaxYear);
    }

    [Fact]
    public void Store_ParsesSubstituteNames()
    {
        var h = HolidayStore.ByDate[new DateOnly(2026, 8, 17)].Single();
        Assert.Equal("대체공휴일(광복절)", h.Name);
        Assert.True(h.IsSubstitute);
        Assert.Equal("광복절", h.SubstituteFor);
    }

    [Fact]
    public void Store_KeepsMultipleHolidaysOnSameDay()
    {
        var names = HolidayStore.ByDate[new DateOnly(2025, 5, 5)].Select(h => h.Name).ToArray();
        Assert.Equal(new[] { "어린이날", "부처님 오신 날" }, names);
    }

    [Fact]
    public void Store_AllSortedIsAscendingByDate()
    {
        var dates = HolidayStore.AllSorted.Select(h => h.Date).ToArray();
        var sorted = dates.OrderBy(d => d).ToArray();
        Assert.Equal(sorted, dates);
    }
}
