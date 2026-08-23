namespace LanguageFeaturesCSharp;

// struct: Werttyp - wird bei Zuweisung kopiert, im Gegensatz zu einer Klasse (Referenztyp),
// bei der zwei Variablen dieselbe Instanz teilen wuerden.
internal struct Punkt
{
    public int X { get; set; }

    public int Y { get; set; }

    public Punkt(int x, int y)
    {
        X = x;
        Y = y;
    }
}

internal static class Structs
{
    public static void Zeigen()
    {
        Punkt punktA = new Punkt(1, 2);
        Punkt punktB = punktA; // erzeugt eine Kopie, nicht dieselbe Instanz

        punktB.X = 99;

        int punktAX = punktA.X; // bleibt unveraendert, weil punktB eine eigene Kopie ist
        int punktBX = punktB.X;

        Console.WriteLine(punktAX);
        Console.WriteLine(punktBX);

        // Liste von structs: Select berechnet aus jedem Punkt einen neuen Wert,
        // die urspruengliche Liste bleibt dabei unveraendert
        List<Punkt> punkte = new List<Punkt>
        {
            new Punkt(1, 2),
            new Punkt(-3, 4),
            new Punkt(5, -1),
        };

        List<double> entfernungenVomUrsprung = punkte
            .Select(p => Math.Sqrt((p.X * p.X) + (p.Y * p.Y)))
            .ToList();

        Punkt naechsterPunkt = punkte
            .OrderBy(p => Math.Sqrt((p.X * p.X) + (p.Y * p.Y)))
            .First();

        Console.WriteLine(string.Join(", ", entfernungenVomUrsprung));
        Console.WriteLine($"{naechsterPunkt.X}, {naechsterPunkt.Y}");
    }
}
