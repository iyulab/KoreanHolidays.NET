namespace KoreanHolidays;

/// <summary>대한민국 공휴일 하나를 나타낸다.</summary>
public sealed record Holiday
{
    /// <summary>공휴일 날짜 (KST 기준 달력 날짜).</summary>
    public required DateOnly Date { get; init; }

    /// <summary>공휴일 명칭. 예: "어린이날", "대체공휴일(광복절)".</summary>
    public required string Name { get; init; }

    /// <summary>대체공휴일 여부.</summary>
    public bool IsSubstitute { get; init; }

    /// <summary>대체공휴일이면 원래 공휴일명("광복절"), 아니면 <see langword="null"/>.</summary>
    public string? SubstituteFor { get; init; }
}
