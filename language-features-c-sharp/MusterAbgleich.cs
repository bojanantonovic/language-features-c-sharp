namespace Language_Features_C_Sharp;

internal static class MusterAbgleich
{
    public static void Zeigen()
    {
        int note = 2;

        // switch-Expression: kompaktere Alternative zum switch-Statement, liefert direkt einen Wert.
        // "_" ist der Auffangfall (entspricht default).
        string bewertung = note switch
        {
            1 => "Sehr gut",
            2 => "Gut",
            3 => "Befriedigend",
            _ => "Unbekannt"
        };

        Console.WriteLine(bewertung);

        // Type Pattern: prueft gleichzeitig den Typ und weist ihn einer Variablen zu (hier: Hund hund)
        Tier tier = new Hund("Rex");
        string beschreibung = tier switch
        {
            Hund hund => $"Hund namens {hund.Name}",
            Katze katze => $"Katze namens {katze.Name}",
            _ => "Unbekanntes Tier"
        };

        Console.WriteLine(beschreibung);

        // Property Pattern: prueft Eigenschaften eines Objekts direkt im Pattern,
        // hier kombiniert mit einem Relational Pattern (< 30)
        Punkt punkt = new Punkt(1, 2);
        string lage = punkt switch
        {
            { X: 0, Y: 0 } => "Ursprung",
            { X: > 0, Y: > 0 } => "Erster Quadrant",
            _ => "Ausserhalb des ersten Quadranten"
        };

        Console.WriteLine(lage);

        // is-Pattern: kombiniert Typpruefung und Bedingung in einem Ausdruck
        object wert = 42;
        if (wert is int zahl and > 10)
        {
            Console.WriteLine(zahl);
        }

        // Liste gemischter Tiere: LINQ mit Pattern in der Lambda zaehlt nur die Hunde
        List<Tier> tiere = new List<Tier> { new Hund("Rex"), new Katze("Minka"), new Hund("Bello") };
        int anzahlHunde = tiere.Count(t => t is Hund);

        Console.WriteLine(anzahlHunde);
    }
}
