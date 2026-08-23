namespace LanguageFeaturesCSharp;

// Enum: benannte Menge fester Werte
internal enum Wochentag
{
    Montag,
    Dienstag,
    Mittwoch,
    Donnerstag,
    Freitag,
    Samstag,
    Sonntag
}

internal static class Enums
{
    public static void Zeigen()
    {
        Wochentag heute = Wochentag.Mittwoch;

        // switch-Statement: prueft den Wert und fuehrt den passenden Fall aus
        string art;
        switch (heute)
        {
            case Wochentag.Samstag:
            case Wochentag.Sonntag:
                art = "Wochenende";
                break;
            default:
                art = "Werktag";
                break;
        }

        Console.WriteLine(heute);
        Console.WriteLine(art);

        // Enum.GetValues<T>() liefert alle Werte des Enums, LINQ funktioniert genauso darauf
        List<Wochentag> alleTage = Enum.GetValues<Wochentag>().ToList();

        List<Wochentag> wochenendTage = alleTage
            .Where(tag => tag == Wochentag.Samstag || tag == Wochentag.Sonntag)
            .ToList();

        int anzahlWerktage = alleTage.Count(tag => tag != Wochentag.Samstag && tag != Wochentag.Sonntag);

        Console.WriteLine(string.Join(", ", wochenendTage));
        Console.WriteLine(anzahlWerktage);
    }
}
