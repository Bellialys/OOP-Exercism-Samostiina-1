using System;

public class Clock : IEquatable<Clock>
{
    private const int MinutesPerDay = 1440;
    private readonly int totalMinutes;
    public Clock(int hours, int minutes) : this((long)hours * 60 + minutes) { }
    private Clock(long minutes) => totalMinutes = (int)((minutes % MinutesPerDay + MinutesPerDay) % MinutesPerDay);
    public Clock Add(int minutesToAdd) => new Clock((long)totalMinutes + minutesToAdd);
    public Clock Subtract(int minutesToSubtract) => new Clock((long)totalMinutes - minutesToSubtract);
    public override string ToString() => $"{totalMinutes / 60:D2}:{totalMinutes % 60:D2}";
    public bool Equals(Clock? other) => other is not null && totalMinutes == other.totalMinutes;
    public override bool Equals(object? obj) => obj is Clock other && Equals(other);
    public override int GetHashCode() => totalMinutes.GetHashCode();
}
