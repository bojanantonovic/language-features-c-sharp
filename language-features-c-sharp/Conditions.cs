namespace LanguageFeaturesCSharp;

internal static class Conditions
{
    public static void Show()
    {
        int age = 30;
        bool isActive = true;

        // if/else on a bool: only two possibilities
        string status;
        if (isActive)
        {
            status = "active";
        }
        else
        {
            status = "inactive";
        }

        // if/else if/else: check several cases in sequence
        string ageGroup;
        if (age < 18)
        {
            ageGroup = "minor";
        }
        else if (age < 65)
        {
            ageGroup = "adult";
        }
        else
        {
            ageGroup = "senior";
        }

        Console.WriteLine(status);
        Console.WriteLine(ageGroup);
    }
}
