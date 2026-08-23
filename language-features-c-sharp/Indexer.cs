namespace LanguageFeaturesCSharp;

// Custom class with an indexer: allows square-bracket access like an array,
// even though a dictionary holds the data internally.
internal class WeeklySchedule
{
    private readonly Dictionary<string, string> appointmentsByDay = new();

    // Indexer: defines this[...], get reads, set writes a value under the given key
    public string this[string day]
    {
        get => appointmentsByDay.TryGetValue(day, out string? appointment) ? appointment : "free";
        set => appointmentsByDay[day] = value;
    }
}

internal static class Indexer
{
    public static void Show()
    {
        WeeklySchedule schedule = new WeeklySchedule();

        // Assignment via the indexer, just like an array
        schedule["Monday"] = "Dentist";
        schedule["Wednesday"] = "Meeting";

        // Reading via the indexer
        string mondayAppointment = schedule["Monday"];
        string tuesdayAppointment = schedule["Tuesday"]; // no entry present -> "free"

        Console.WriteLine(mondayAppointment);
        Console.WriteLine(tuesdayAppointment);

        // List of days: LINQ uses the indexer to look up the appointment for each day
        List<string> days = new List<string> { "Monday", "Tuesday", "Wednesday" };

        List<string> plan = days.Select(day => $"{day}: {schedule[day]}").ToList();

        Console.WriteLine(string.Join(" | ", plan));
    }
}
