namespace LanguageFeaturesCSharp;

// Extension-Methoden muessen in einer static Klasse stehen. Der erste Parameter mit
// "this" davor legt fest, welchen Typ die Methode erweitert (hier: string).
internal static class StringErweiterungen
{
    public static string Umdrehen(this string text)
    {
        char[] zeichen = text.ToCharArray();
        Array.Reverse(zeichen);
        return new string(zeichen);
    }

    public static bool IstPalindrom(this string text)
    {
        string umgedreht = text.Umdrehen();
        return string.Equals(text, umgedreht, StringComparison.OrdinalIgnoreCase);
    }
}

internal static class Extensions
{
    public static void Zeigen()
    {
        string wort = "Anna";

        // sieht wie ein ganz normaler Methodenaufruf auf string aus,
        // obwohl string selbst nicht veraendert wurde
        bool istPalindrom = wort.IstPalindrom();
        string umgedreht = wort.Umdrehen();

        Console.WriteLine(istPalindrom);
        Console.WriteLine(umgedreht);

        // Liste von Woertern: die Extension-Methoden lassen sich direkt in LINQ-Lambdas verwenden
        List<string> woerter = new List<string> { "Anna", "Otto", "Haus", "Level", "Baum" };

        List<string> palindrome = woerter.Where(w => w.IstPalindrom()).ToList();
        List<string> umgedrehteWoerter = woerter.Select(w => w.Umdrehen()).ToList();

        Console.WriteLine(string.Join(", ", palindrome));
        Console.WriteLine(string.Join(", ", umgedrehteWoerter));
    }
}
