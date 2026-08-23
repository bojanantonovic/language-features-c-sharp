namespace LanguageFeaturesCSharp;

// Eigene Klasse mit Indexer: erlaubt den Zugriff mit eckigen Klammern wie bei einem Array,
// obwohl intern ein Dictionary die Daten haelt.
internal class Wochenplan
{
    private readonly Dictionary<string, string> termineNachTag = new();

    // Indexer: definiert this[...], get liest, set schreibt einen Wert unter dem angegebenen Schluessel
    public string this[string tag]
    {
        get => termineNachTag.TryGetValue(tag, out string? termin) ? termin : "frei";
        set => termineNachTag[tag] = value;
    }
}

internal static class Indexer
{
    public static void Zeigen()
    {
        Wochenplan plan = new Wochenplan();

        // Zuweisung ueber den Indexer, genau wie bei einem Array
        plan["Montag"] = "Zahnarzt";
        plan["Mittwoch"] = "Meeting";

        // Lesen ueber den Indexer
        string montagTermin = plan["Montag"];
        string dienstagTermin = plan["Dienstag"]; // kein Eintrag vorhanden -> "frei"

        Console.WriteLine(montagTermin);
        Console.WriteLine(dienstagTermin);

        // Liste von Tagen: LINQ nutzt den Indexer, um pro Tag den Termin abzufragen
        List<string> tage = new List<string> { "Montag", "Dienstag", "Mittwoch" };

        List<string> terminplan = tage.Select(tag => $"{tag}: {plan[tag]}").ToList();

        Console.WriteLine(string.Join(" | ", terminplan));
    }
}
