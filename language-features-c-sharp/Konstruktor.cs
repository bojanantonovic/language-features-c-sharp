namespace LanguageFeaturesCSharp;

// Klasse mit Konstruktoren: Eigenschaften werden direkt bei der Erzeugung des Objekts gesetzt.
internal class Buch
{
    public string Titel { get; set; }

    public int Seitenzahl { get; set; }

    // Konstruktor mit Parametern
    public Buch(string titel, int seitenzahl)
    {
        Titel = titel;
        Seitenzahl = seitenzahl;
    }

    // Ueberladener Konstruktor: ruft den anderen Konstruktor per "this(...)" auf
    // und setzt dabei einen Standardwert fuer die Seitenzahl.
    public Buch(string titel) : this(titel, 0)
    {
    }
}

internal static class Konstruktor
{
    public static void Zeigen()
    {
        Buch roman = new Buch("Der Steppenwolf", 320);
        Buch unbekannt = new Buch("Unbekanntes Buch");

        string romanTitel = roman.Titel;
        int romanSeitenzahl = roman.Seitenzahl;

        Console.WriteLine(romanTitel);
        Console.WriteLine(romanSeitenzahl);
        Console.WriteLine(unbekannt.Titel);
        Console.WriteLine(unbekannt.Seitenzahl);
    }
}
