namespace LanguageFeaturesCSharp;

// struct mit ueberladenen Operatoren: definiert, was "+" und "==" fuer diesen eigenen Typ bedeuten
internal struct Vektor
{
    public int X { get; }

    public int Y { get; }

    public Vektor(int x, int y)
    {
        X = x;
        Y = y;
    }

    // operator +: legt fest, wie zwei Vektor-Werte addiert werden
    public static Vektor operator +(Vektor a, Vektor b)
    {
        return new Vektor(a.X + b.X, a.Y + b.Y);
    }

    // operator ==/!=: eigene Gleichheitspruefung anhand der Werte statt der Referenz
    public static bool operator ==(Vektor a, Vektor b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vektor a, Vektor b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vektor andererVektor && this == andererVektor;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

internal static class Operatoren
{
    public static void Zeigen()
    {
        Vektor a = new Vektor(1, 2);
        Vektor b = new Vektor(3, 4);

        // ruft operator + auf, obwohl Vektor eine selbst geschriebene Struktur ist
        Vektor summe = a + b;

        // ruft operator == auf: vergleicht X und Y, nicht die Referenz
        bool sindGleich = a == b;
        bool sindUngleich = a != b;

        Console.WriteLine(summe);
        Console.WriteLine(sindGleich);
        Console.WriteLine(sindUngleich);

        // Liste von Vektoren: Aggregate nutzt wiederholt operator + auf, um alle zu addieren
        List<Vektor> vektoren = new List<Vektor> { new Vektor(1, 1), new Vektor(2, 3), new Vektor(-1, 4) };

        Vektor gesamtsumme = vektoren.Aggregate((v1, v2) => v1 + v2);

        Console.WriteLine(gesamtsumme);
    }
}
