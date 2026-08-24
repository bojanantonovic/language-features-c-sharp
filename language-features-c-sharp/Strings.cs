namespace LanguageFeaturesCSharp;

internal static class Strings
{
    public static void Show()
    {
        string name = "Alice";
        int age = 30;
        double price = 19.999;

        // String interpolation: insert values directly into the string, with $ before the quotes
        string greeting = $"Hello, {name}!";

        // Calculations are also possible inside {}
        string ageInfo = $"{name} is {age} years old, in 10 years {age + 10}.";

        // Formatting inside {}: :F2 rounds to 2 decimal places
        string priceText = $"Price: {price:F2}";

        Console.WriteLine(greeting);
        Console.WriteLine(ageInfo);
        Console.WriteLine(priceText);

        // Verbatim string (@"..."): escape characters like \ are not interpreted, useful for paths
        string path = @"C:\Data\Alice\Notes.txt";

        // Split: breaks a string into several parts at a separator
        string csv = "Apple, Pear , Cherry";
        string[] parts = csv.Split(',');

        // Trim removes whitespace at the start/end of each part
        List<string> trimmedParts = parts.Select(part => part.Trim()).ToList();

        // Join: combines several parts back into one string
        string joined = string.Join(" | ", trimmedParts);

        // Substring checks
        bool containsPear = joined.Contains("Pear");
        bool startsWithApple = joined.StartsWith("Apple");

        Console.WriteLine(path);
        Console.WriteLine(joined);
        Console.WriteLine(containsPear);
        Console.WriteLine(startsWithApple);
    }
}
