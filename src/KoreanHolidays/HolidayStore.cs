using System.Collections.Frozen;
using System.Collections.Immutable;

namespace KoreanHolidays;

internal static class HolidayStore
{
    internal static int MinYear => HolidayData.MinYear;
    internal static int MaxYear => HolidayData.MaxYear;

    /// <summary>날짜 오름차순으로 정렬된 전체 공휴일. 같은 날은 원본 배열 순서를 유지한다.</summary>
    internal static readonly ImmutableArray<Holiday> AllSorted =
        [.. HolidayData.CreateAll().OrderBy(h => h.Date)];

    /// <summary>날짜별 공휴일 조회용 동결 사전.</summary>
    internal static readonly FrozenDictionary<DateOnly, ImmutableArray<Holiday>> ByDate =
        AllSorted
            .GroupBy(h => h.Date)
            .ToFrozenDictionary(g => g.Key, g => g.ToImmutableArray());
}
