namespace LanguageFeaturesCSharp.Generics;

internal static class Generics
{
    public static void Show()
    {
        // The same Greater<T> method works for int and string,
        // without having to be rewritten for each type.
        int greaterNumber = Greater(5, 12);
        string greaterName = Greater("Anna", "Bob");

        Console.WriteLine(greaterNumber);
        Console.WriteLine(greaterName);
    }

    // Generic method: T is a placeholder for any type
    // that implements IComparable<T> (e.g. int, double, string).
    private static T Greater<T>(T a, T b) where T : IComparable<T>
    {
        if (a.CompareTo(b) > 0)
        {
            return a;
        }

        return b;
    }
}
