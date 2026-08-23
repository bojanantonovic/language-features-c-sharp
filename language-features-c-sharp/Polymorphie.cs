namespace LanguageFeaturesCSharp;

internal static class Polymorphie
{
    public static void Zeigen()
    {
        // Liste vom Basistyp Tier, enthaelt aber Objekte unterschiedlicher abgeleiteter Typen
        List<Tier> tiere = new List<Tier>();
        tiere.Add(new Hund("Rex"));
        tiere.Add(new Katze("Minka"));
        tiere.Add(new Hund("Bello"));

        // Polymorphie: derselbe Aufruf MachGeraeusch() liefert je nach tatsaechlichem Typ
        // ein anderes Ergebnis - der Compiler kennt hier nur Tier, zur Laufzeit wird aber
        // die passende override-Methode von Hund bzw. Katze ausgefuehrt.
        foreach (Tier tier in tiere)
        {
            string geraeusch = tier.MachGeraeusch();
            Console.WriteLine(tier.Name);
            Console.WriteLine(geraeusch);
        }
    }
}
