namespace LanguageFeaturesCSharp;

internal static class Polymorphismus
{
    public static void Zeigen()
    {
        // Delegate, der auf die (polymorphe) Methode MachGeraeusch zeigt
        Func<Tier, string> geraeuschVon = tier => tier.MachGeraeusch();

        Tier hund = new Hund("Rex");
        Tier katze = new Katze("Minka");

        // Derselbe Delegate ruft je nach tatsaechlichem Typ eine andere Implementierung auf -
        // die Polymorphie aus Vererbung.cs/Polymorphie.cs wirkt genauso durch einen Delegate hindurch.
        string hundGeraeusch = geraeuschVon(hund);
        string katzeGeraeusch = geraeuschVon(katze);

        Console.WriteLine(hundGeraeusch);
        Console.WriteLine(katzeGeraeusch);

        // Delegates koennen aber auch direkt unterschiedliches Verhalten tragen,
        // ganz ohne Vererbung - eine zweite, unabhaengige Form von "austauschbarem Verhalten".
        Func<Tier, string> lauteVersion = tier => tier.MachGeraeusch().ToUpper() + "!!!";
        string lauterHund = lauteVersion(hund);

        Console.WriteLine(lauterHund);
    }
}
