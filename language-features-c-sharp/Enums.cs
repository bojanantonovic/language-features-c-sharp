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

        // switch statement: checks the value and executes the matching case
        string kind;
        switch (today)
        {
            case Weekday.Saturday:
            case Weekday.Sunday:
                kind = "Weekend";
                break;
            default:
                kind = "Workday";
                break;
        }

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
