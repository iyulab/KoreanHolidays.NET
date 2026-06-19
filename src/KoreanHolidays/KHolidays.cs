using System.Collections.Immutable;

namespace KoreanHolidays;

/// <summary>대한민국 공휴일 조회 진입점. <c>using KoreanHolidays;</c> 후 <c>KHolidays.IsPublicHoliday(date)</c> 형태로 사용한다.</summary>
public static class KHolidays
{
    /// <summary>데이터가 존재하는 가장 이른 연도.</summary>
    public static int MinYear => HolidayStore.MinYear;

    /// <summary>데이터가 존재하는 가장 늦은 연도.</summary>
    public static int MaxYear => HolidayStore.MaxYear;

    /// <summary>해당 연도가 데이터 범위 내인지 여부.</summary>
    public static bool IsSupportedYear(int year) => year >= MinYear && year <= MaxYear;

    /// <summary>해당 날짜가 공휴일인지 여부.</summary>
    /// <exception cref="ArgumentOutOfRangeException">데이터 범위 밖 연도일 때.</exception>
    public static bool IsPublicHoliday(DateOnly date)
    {
        EnsureSupported(date.Year, nameof(date));
        return HolidayStore.ByDate.ContainsKey(date);
    }

    /// <summary>해당 날짜의 공휴일 목록. 공휴일이 아니면 빈 리스트.</summary>
    /// <exception cref="ArgumentOutOfRangeException">데이터 범위 밖 연도일 때.</exception>
    public static IReadOnlyList<Holiday> GetHolidays(DateOnly date)
    {
        EnsureSupported(date.Year, nameof(date));
        return HolidayStore.ByDate.TryGetValue(date, out var holidays)
            ? holidays
            : ImmutableArray<Holiday>.Empty;
    }

    /// <summary>
    /// 해당 날짜에 공휴일이 하나 이상 있으면 <see langword="true"/>와 목록을 반환한다.
    /// 미지원 연도이거나 공휴일이 없으면 <see langword="false"/>와 빈 리스트를 반환한다(예외 없음).
    /// </summary>
    public static bool TryGetHolidays(DateOnly date, out IReadOnlyList<Holiday> holidays)
    {
        if (IsSupportedYear(date.Year) && HolidayStore.ByDate.TryGetValue(date, out var found))
        {
            holidays = found;
            return true;
        }

        holidays = ImmutableArray<Holiday>.Empty;
        return false;
    }

    /// <summary>해당 연도의 모든 공휴일을 날짜 오름차순으로 반환한다.</summary>
    /// <exception cref="ArgumentOutOfRangeException">데이터 범위 밖 연도일 때.</exception>
    public static IReadOnlyList<Holiday> GetHolidays(int year)
    {
        EnsureSupported(year, nameof(year));
        return [.. HolidayStore.AllSorted.Where(h => h.Date.Year == year)];
    }

    /// <summary><paramref name="start"/>부터 <paramref name="end"/>까지(양끝 포함) 모든 공휴일을 날짜 오름차순으로 반환한다.</summary>
    /// <exception cref="ArgumentException"><paramref name="start"/>가 <paramref name="end"/>보다 클 때.</exception>
    /// <exception cref="ArgumentOutOfRangeException">구간이 데이터 범위 밖 연도를 포함할 때.</exception>
    public static IReadOnlyList<Holiday> GetHolidaysInRange(DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new ArgumentException($"start({start})가 end({end})보다 늦습니다.", nameof(start));

        EnsureSupported(start.Year, nameof(start));
        EnsureSupported(end.Year, nameof(end));
        return [.. HolidayStore.AllSorted.Where(h => h.Date >= start && h.Date <= end)];
    }

    private static void EnsureSupported(int year, string paramName)
    {
        if (!IsSupportedYear(year))
            throw new ArgumentOutOfRangeException(paramName, year,
                $"공휴일 데이터가 없는 연도입니다. 지원 범위: {MinYear}~{MaxYear}.");
    }
}
