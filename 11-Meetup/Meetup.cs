using System;

public enum Schedule { Teenth, First, Second, Third, Fourth, Last }
public class Meetup
{
    private readonly int month, year;
    public Meetup(int month, int year)
    {
        this.month = month;
        this.year = year;
    }
    public DateTime Day(DayOfWeek dayOfWeek, Schedule schedule)
    {
        int startDay = schedule switch
        {
            Schedule.First => 1, Schedule.Second => 8, Schedule.Third => 15,
            Schedule.Fourth => 22, Schedule.Teenth => 13,
            Schedule.Last => DateTime.DaysInMonth(year, month) - 6,
            _ => throw new ArgumentOutOfRangeException(nameof(schedule))
        };
        DateTime date = new DateTime(year, month, startDay);
        int offset = ((int)dayOfWeek - (int)date.DayOfWeek + 7) % 7;
        return date.AddDays(offset);
    }
}
