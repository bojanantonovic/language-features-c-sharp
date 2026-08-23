namespace LanguageFeaturesCSharp;

internal static class Generics
{
    public static void Zeigen()
    {
        // Dieselbe Methode Groesser<T> funktioniert fuer int und string,
        // ohne dass sie fuer jeden Typ neu geschrieben werden muss.
        int groessereZahl = Groesser(5, 12);
        string groessererName = Groesser("Anna", "Bob");

        Console.WriteLine(groessereZahl);
        Console.WriteLine(groessererName);
    }

    // Generische Methode: T ist ein Platzhalter fuer einen beliebigen Typ,
    // der IComparable<T> implementiert (z. B. int, double, string).
    private static T Groesser<T>(T a, T b) where T : IComparable<T>
    {
        if (a.CompareTo(b) > 0)
        {
            return a;
        }

        return b;
    }
}
