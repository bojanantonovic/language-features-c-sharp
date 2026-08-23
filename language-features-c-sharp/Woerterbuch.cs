namespace Language_Features_C_Sharp;

internal static class Woerterbuch
{
    public static void Zeigen()
    {
        // Dictionary: speichert Werte unter einem eindeutigen Schluessel
        Dictionary<string, int> alterNachName = new();
        alterNachName["Alice"] = 30;
        alterNachName["Bob"] = 25;
        alterNachName.Add("Carol", 40);

        // Zugriff ueber den Schluessel
        int aliceAlter = alterNachName["Alice"];

        // Sicherer Zugriff: TryGetValue liefert true/false statt bei fehlendem Schluessel
        // eine Exception zu werfen
        bool gefunden = alterNachName.TryGetValue("Dave", out int daveAlter);

        int anzahlEintraege = alterNachName.Count;

        Console.WriteLine(aliceAlter);
        Console.WriteLine(gefunden);
        Console.WriteLine(daveAlter);
        Console.WriteLine(anzahlEintraege);

        foreach (KeyValuePair<string, int> eintrag in alterNachName)
        {
            Console.WriteLine(eintrag.Key);
            Console.WriteLine(eintrag.Value);
        }

        // LINQ auf einem Dictionary: iteriert ueber KeyValuePair<string, int>-Eintraege
        List<string> namenUeber28 = alterNachName
            .Where(eintrag => eintrag.Value > 28)
            .Select(eintrag => eintrag.Key)
            .ToList();

        // OrderByDescending + First: den Namen mit dem hoechsten Alter finden
        string aeltesterName = alterNachName
            .OrderByDescending(eintrag => eintrag.Value)
            .First()
            .Key;

        Console.WriteLine(string.Join(", ", namenUeber28));
        Console.WriteLine(aeltesterName);
    }
}
