namespace LanguageFeaturesCSharp;

// Basisklasse
internal class Tier
{
    public string Name { get; set; }

    public Tier(string name)
    {
        Name = name;
    }

    // virtual: darf von abgeleiteten Klassen ueberschrieben werden
    public virtual string MachGeraeusch()
    {
        return "...";
    }
}

// Abgeleitete Klasse: erbt von Tier, "base(...)" ruft den Konstruktor der Basisklasse auf
internal class Hund : Tier
{
    public Hund(string name) : base(name)
    {
    }

    // override: ersetzt die Basisimplementierung von MachGeraeusch
    public override string MachGeraeusch()
    {
        return "Wuff";
    }
}

internal class Katze : Tier
{
    public Katze(string name) : base(name)
    {
    }

    public override string MachGeraeusch()
    {
        return "Miau";
    }
}

internal static class Vererbung
{
    public static void Zeigen()
    {
        Hund hund = new Hund("Rex");
        Katze katze = new Katze("Minka");

        string hundGeraeusch = hund.MachGeraeusch();
        string katzeGeraeusch = katze.MachGeraeusch();

        Console.WriteLine(hund.Name);
        Console.WriteLine(hundGeraeusch);
        Console.WriteLine(katze.Name);
        Console.WriteLine(katzeGeraeusch);

        // Liste vom abgeleiteten Typ Hund: LINQ nutzt die von Tier geerbte Eigenschaft Name direkt
        List<Hund> hunde = new List<Hund>
        {
            new Hund("Rex"),
            new Hund("Bello"),
            new Hund("Ari"),
        };

        List<string> hundeNamenSortiert = hunde
            .OrderBy(h => h.Name)
            .Select(h => h.Name)
            .ToList();

        Console.WriteLine(string.Join(", ", hundeNamenSortiert));
    }
}
