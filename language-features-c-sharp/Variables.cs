namespace LanguageFeaturesCSharp;

internal static class Variables
{
    public static void Show()
    {
        // Integer
        int age = 30;

        // Floating-point number
        double height = 1.78;

        // Text
        string name = "Alice";

        // Boolean: true or false
        bool isActive = true;

        // A calculation: the result lands in its own variable first,
        // not directly inside the Console.WriteLine line.
        int ageInTenYears = age + 10;

        Console.WriteLine(name);
        Console.WriteLine(age);
        Console.WriteLine(height);
        Console.WriteLine(isActive);
        Console.WriteLine(ageInTenYears);
    }
}
