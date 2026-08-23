namespace Language_Features_C_Sharp;

internal static class Iteratoren
{
    public static void Zeigen()
    {
        // yield return erzeugt die Werte erst nacheinander, wenn sie tatsaechlich abgefragt werden -
        // die Methode selbst laeuft nicht sofort komplett durch.
        foreach (int zahl in GeradeZahlenBis(10))
        {
            Console.WriteLine(zahl);
        }

        // Das Ergebnis eines Iterators laesst sich wie jede andere IEnumerable<T> mit LINQ weiterverarbeiten
        List<int> geradeZahlen = GeradeZahlenBis(10).ToList();
        int summe = geradeZahlen.Sum();

        Console.WriteLine(summe);

        // Eigener Iterator ueber eine Liste: liefert die Elemente in umgekehrter Reihenfolge
        List<string> namen = new List<string> { "Alice", "Bob", "Carol" };

        foreach (string name in Rueckwaerts(namen))
        {
            Console.WriteLine(name);
        }
    }

    // IEnumerable<T> als Rueckgabetyp + yield return: der Aufrufer bekommt die Werte einzeln,
    // ohne dass vorher eine komplette Liste im Speicher aufgebaut werden muss.
    private static IEnumerable<int> GeradeZahlenBis(int obergrenze)
    {
        for (int zahl = 0; zahl <= obergrenze; zahl++)
        {
            if (zahl % 2 == 0)
            {
                yield return zahl;
            }
        }
    }

    private static IEnumerable<string> Rueckwaerts(List<string> werte)
    {
        for (int index = werte.Count - 1; index >= 0; index--)
        {
            yield return werte[index];
        }
    }
}
