namespace LanguageFeaturesCSharp;

internal static class Strings
{
    public static void Zeigen()
    {
        string name = "Alice";
        int alter = 30;
        double preis = 19.999;

        // String-Interpolation: Werte direkt im String einsetzen, mit $ vor den Anfuehrungszeichen
        string begruessung = $"Hallo, {name}!";

        // Auch Berechnungen sind innerhalb von {} moeglich
        string altersinfo = $"{name} ist {alter} Jahre alt, in 10 Jahren {alter + 10}.";

        // Formatierung innerhalb von {}: :F2 rundet auf 2 Nachkommastellen
        string preisText = $"Preis: {preis:F2}";

        Console.WriteLine(begruessung);
        Console.WriteLine(altersinfo);
        Console.WriteLine(preisText);

        // Verbatim-String (@"..."): Escape-Zeichen wie \ werden nicht interpretiert, nuetzlich fuer Pfade
        string pfad = @"C:\Daten\Alice\Notizen.txt";

        // Split: zerlegt einen String an einem Trennzeichen in mehrere Teile
        string csv = "Apfel, Birne , Kirsche";
        string[] teile = csv.Split(',');

        // Trim entfernt Leerzeichen am Anfang/Ende jedes Teils
        List<string> getrimmteTeile = new List<string>();
        foreach (string teil in teile)
        {
            getrimmteTeile.Add(teil.Trim());
        }

        // Join: fuegt mehrere Teile wieder zu einem String zusammen
        string zusammengefuegt = string.Join(" | ", getrimmteTeile);

        // Pruefungen auf Teilstrings
        bool enthaeltBirne = zusammengefuegt.Contains("Birne");
        bool startetMitApfel = zusammengefuegt.StartsWith("Apfel");

        Console.WriteLine(pfad);
        Console.WriteLine(zusammengefuegt);
        Console.WriteLine(enthaeltBirne);
        Console.WriteLine(startetMitApfel);
    }
}
