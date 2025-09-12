using GymManagement.Domain.Abstractions;

namespace GymManagement.Domain.Entities.OperatingHoursModel;

public sealed record TimeRange
{
    public TimeOnly From { get; }
    public TimeOnly To { get; }

    private TimeRange(TimeOnly from, TimeOnly to)
    {
        From = from;
        To = to;
    }

    public static Result<TimeRange> Create(TimeOnly from, TimeOnly to)
    {
        if (to <= from)
            return Result.Failure<TimeRange>(new Error("TimeRange.Invalid", "????? ????????? ?????? ???? ?????? ??????? ??????"));

        return Result.Success(new TimeRange(from, to));
    }

    // EF Core
    private TimeRange() { }
}

public sealed record DaySchedule
{
    public DayOfWeek Day { get; }
    public IReadOnlyList<TimeRange> WorkingSpans => _workingSpans;

    private readonly List<TimeRange> _workingSpans = [];

    private DaySchedule(DayOfWeek day, IEnumerable<TimeRange> workingSpans)
    {
        Day = day;
        _workingSpans = workingSpans.ToList();
    }

    public static Result<DaySchedule> Create(DayOfWeek day, IEnumerable<TimeRange> spans)
    {
        if (!spans.Any())
            return Result.Failure<DaySchedule>(new Error("DaySchedule.Empty", "?????? ???? ???? ?? ???? ????????"));

        var ordered = spans.OrderBy(s => s.From).ToList();
        for (int i = 1; i < ordered.Count; i++)
        {
            if (ordered[i - 1].To > ordered[i].From)
                return Result.Failure<DaySchedule>(new Error("DaySchedule.Overlap", "????????? ????????????"));
        }

        return Result.Success(new DaySchedule(day, ordered));
    }

    // EF Core
    private DaySchedule() { }
}

public sealed record OperatingHours
{
    public IReadOnlyList<DaySchedule> Days => _days;
    public IReadOnlySet<DateOnly> ClosedDates => _closedDates;

    private readonly List<DaySchedule> _days = [];
    private readonly HashSet<DateOnly> _closedDates = [];

    private OperatingHours(IEnumerable<DaySchedule> days, IEnumerable<DateOnly>? closedDates)
    {
        _days = days.OrderBy(d => d.Day).ToList();
        if (closedDates is not null)
            _closedDates = closedDates.ToHashSet();
    }

    public static Result<OperatingHours> Create(IEnumerable<DaySchedule> days, IEnumerable<DateOnly>? closedDates = null)
    {
        var list = days.ToList();
        if (list.Count == 0)
            return Result.Failure<OperatingHours>(new Error("OperatingHours.Empty", "??? ??????????"));

        if (list.Select(d => d.Day).Distinct().Count() != list.Count)
            return Result.Failure<OperatingHours>(new Error("OperatingHours.DuplicateDay", "????? ????"));

        return Result.Success(new OperatingHours(list, closedDates));
    }

    public bool IsOpenAt(DateTime utc, TimeZoneInfo tz)
    {
        var local = TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
        if (_closedDates.Contains(DateOnly.FromDateTime(local))) return false;
        var schedule = _days.FirstOrDefault(d => d.Day == local.DayOfWeek);
        if (schedule is null) return false;
        return schedule.WorkingSpans.Any(span => local.TimeOfDay >= span.From.ToTimeSpan() && local.TimeOfDay < span.To.ToTimeSpan());
    }

    // EF Core
    private OperatingHours() { }
}
