namespace LanguageFeaturesCSharp;

internal static class Collections
{
    public static void Zeigen()
    {
        List<int> zahlen = new List<int> { 5, 12, 3, 8, 21, 4 };

        // Where: filtert Elemente, die eine Bedingung erfuellen
        List<int> geradeZahlen = zahlen.Where(zahl => zahl % 2 == 0).ToList();

        // Select: wandelt jedes Element um
        List<int> verdoppelt = zahlen.Select(zahl => zahl * 2).ToList();

        // OrderBy: sortiert aufsteigend, ohne die urspruengliche Liste zu veraendern
        List<int> sortiert = zahlen.OrderBy(zahl => zahl).ToList();

        // Aggregat-Funktionen liefern direkt einen einzelnen Wert statt einer Liste
        int summe = zahlen.Sum();
        double durchschnitt = zahlen.Average();
        int groessteZahl = zahlen.Max();

        Console.WriteLine(string.Join(", ", geradeZahlen));
        Console.WriteLine(string.Join(", ", verdoppelt));
        Console.WriteLine(string.Join(", ", sortiert));
        Console.WriteLine(summe);
        Console.WriteLine(durchschnitt);
        Console.WriteLine(groessteZahl);
    }
}
