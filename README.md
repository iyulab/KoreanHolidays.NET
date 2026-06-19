# KoreanHolidays

[![CI](https://github.com/iyulab/KoreanHolidays.NET/actions/workflows/ci.yml/badge.svg)](https://github.com/iyulab/KoreanHolidays.NET/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/KoreanHolidays.svg)](https://www.nuget.org/packages/KoreanHolidays/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

대한민국 공휴일(대체공휴일 포함)을 런타임 의존성 없이 조회하는 .NET 라이브러리. **net8.0 / net9.0 / net10.0** 멀티타깃, 외부 의존성 없음(BCL only).

데이터 출처: [hyunbinseo/holidays-kr](https://github.com/hyunbinseo/holidays-kr) — 우주항공청 발표 **월력요항** 기반.

## API 요약

| 멤버 | 설명 |
|------|------|
| `IsPublicHoliday(DateOnly)` | 공휴일 여부 (미지원 연도 → `ArgumentOutOfRangeException`) |
| `GetHolidays(DateOnly)` | 해당 날짜의 공휴일 목록 (없으면 빈 리스트) |
| `TryGetHolidays(DateOnly, out …)` | 예외 없이 분기 — 미지원 연도/비공휴일이면 `false` |
| `GetHolidays(int year)` | 연도 전체 공휴일 (날짜 오름차순) |
| `GetHolidaysInRange(DateOnly, DateOnly)` | 기간(양끝 포함) 공휴일 (날짜 오름차순) |
| `MinYear` / `MaxYear` / `IsSupportedYear(int)` | 지원 범위 (현재 2018~2026) |

## 설치

```bash
dotnet add package KoreanHolidays
```

## 사용

```csharp
using System;
using System.Linq;
using KoreanHolidays;

// 공휴일 여부
KHolidays.IsPublicHoliday(new DateOnly(2026, 1, 1));  // true
KHolidays.IsPublicHoliday(new DateOnly(2026, 1, 2));  // false

// 공휴일 명칭들 (같은 날 복수 가능)
KHolidays.GetHolidays(new DateOnly(2025, 5, 5))
    .Select(h => h.Name);                              // ["어린이날", "부처님 오신 날"]

// 대체공휴일 정보
var sub = KHolidays.GetHolidays(new DateOnly(2026, 8, 17)).Single();
sub.IsSubstitute;    // true
sub.SubstituteFor;   // "광복절"

// 예외 없이 분기 (Try 패턴)
if (KHolidays.TryGetHolidays(new DateOnly(2026, 1, 1), out var holidays)) { /* ... */ }

// 연도 / 기간 조회
KHolidays.GetHolidays(2026);
KHolidays.GetHolidaysInRange(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31));

// 지원 범위
KHolidays.MinYear;  // 2018
KHolidays.MaxYear;  // 2026
KHolidays.IsSupportedYear(2030);  // false
```

## 동작 정책

- 입력은 시간대 없는 `DateOnly`로 받는다. `DateTime`에서 변환: `DateOnly.FromDateTime(kstDateTime)`.
- 데이터 범위 밖 연도는 "공휴일 없음"이 아니라 "알 수 없음"으로 취급한다.
  단정 API(`IsPublicHoliday`, `GetHolidays`)는 `ArgumentOutOfRangeException`을 던지고,
  `TryGetHolidays`는 `false`를 반환한다.
- 2026년부터 노동절(5월 1일)과 제헌절(7월 17일)이 법정공휴일로 포함된다 (2025년 공휴일법 개정 반영).

## 데이터 갱신

원본 리포의 새 연도가 추가되면:

```bash
# tools/HolidayData.Generator/data/ 에 새 YYYY.json 추가 후
dotnet run --project tools/HolidayData.Generator -- src/KoreanHolidays/Generated/HolidayData.g.cs
```

## 라이선스

MIT
