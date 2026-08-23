namespace LanguageFeaturesCSharp;

internal static class Tupel
{
    public static void Zeigen()
    {
        // Tuple: mehrere Werte zusammen zurueckgeben, ohne dafuer eine eigene Klasse zu brauchen
        (string name, int alter) person = ("Alice", 30);

        Console.WriteLine(person.name);
        Console.WriteLine(person.alter);

        // Deconstruction: die Felder eines Tuples direkt in eigene Variablen entpacken
        (string vorname, int jahre) = person;

        Console.WriteLine(vorname);
        Console.WriteLine(jahre);

        // Tuple als Rueckgabewert einer Methode
        (int min, int max) minMax = MinMax(new List<int> { 5, 12, 3, 8, 21, 4 });

        Console.WriteLine(minMax.min);
        Console.WriteLine(minMax.max);

        // Liste von Tuples: LINQ funktioniert genauso wie bei jedem anderen Typ
        List<(string name, int alter)> personen = new List<(string name, int alter)>
        {
            ("Alice", 30),
            ("Bob", 25),
            ("Carol", 40),
        };

        List<string> namenUeber28 = personen
            .Where(p => p.alter > 28)
            .Select(p => p.name)
            .ToList();

        Console.WriteLine(string.Join(", ", namenUeber28));
    }

    // Named Tuple als Rueckgabetyp: (int min, int max) statt einer eigenen Klasse fuer zwei Werte
    private static (int min, int max) MinMax(List<int> zahlen)
    {
        return (zahlen.Min(), zahlen.Max());
    }
}
