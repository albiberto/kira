namespace Kira.Extensions;

public static class TimeExtensions
{
    public static int ToSeconds(this object? field) =>
        int.TryParse($"{field}", out var value)
            ? value
            : 0;

    public static string ToDaysHoursMinutes(this int? value)
    {
        var seconds = value ?? 0;

        // Calculate days, hours, minutes, and seconds
        var days = seconds / 86400; // There are 86400 seconds in a day
        var remainingSeconds = seconds % 86400; // Calculate the remaining seconds after calculating days
        var hours = remainingSeconds / 3600; // There are 3600 seconds in an hour
        var remainingSeconds2 = remainingSeconds % 3600; // Calculate the remaining seconds after calculating hours
        var minutes = remainingSeconds2 / 60; // There are 60 seconds in a minute
        var remainingSeconds3 = remainingSeconds2 % 60; // Calculate the remaining seconds after calculating minutes

        return $"{days}d {hours}h {minutes}m";
    }

    public static string ToWorkingDaysHoursMinutes(this int? value)
    {
        var seconds = value ?? 0;

        // Calculate working days, hours, minutes, and seconds
        var workingDays = seconds / 28800; // There are 28,800 seconds in an 8-hour workday
        var remainingSeconds = seconds % 28800; // Calculate the remaining seconds after calculating working days
        var hours = remainingSeconds / 3600; // There are 3,600 seconds in an hour
        var remainingSeconds2 = remainingSeconds % 3600; // Calculate the remaining seconds after calculating hours
        var minutes = remainingSeconds2 / 60; // There are 60 seconds in a minute
        var remainingSeconds3 = remainingSeconds2 % 60; // Calculate the remaining seconds after calculating minutes

        return $"{workingDays}d {hours}h {minutes}m";
    }
}