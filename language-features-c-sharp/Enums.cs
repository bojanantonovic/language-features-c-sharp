using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp;

// Enum: named set of fixed values
internal enum Weekday
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

internal static class Enums
{
    public static void Show()
    {
        Weekday today = Weekday.Wednesday;

        // switch expression: more compact than a switch statement, returns a value directly.
        // "or" combines several cases in one arm instead of falling through.
        string kind = today switch
        {
            Weekday.Saturday or Weekday.Sunday => "Weekend",
            _ => "Workday"
        };

        Console.WriteLine(today);
        Console.WriteLine(kind);

        // Enum.GetValues<T>() returns all values of the enum, LINQ works on it just the same
        List<Weekday> allDays = Enum.GetValues<Weekday>().ToList();

        List<Weekday> weekendDays = allDays
            .Where(day => day == Weekday.Saturday || day == Weekday.Sunday)
            .ToList();

        int workdayCount = allDays.Count(day => day != Weekday.Saturday && day != Weekday.Sunday);

        Console.WriteLine(string.Join(", ", weekendDays));
        Console.WriteLine(workdayCount);
    }
}
