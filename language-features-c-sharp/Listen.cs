namespace LanguageFeaturesCSharp;

internal static class Listen
{
    public static void Zeigen()
    {
        // Array: feste Groesse, direkt bei der Deklaration befuellt
        string[] farben = { "Rot", "Gruen", "Blau" };
        string ersteFarbe = farben[0];
        int anzahlFarben = farben.Length;

        // List<T>: veraenderliche Groesse, Elemente koennen hinzugefuegt/entfernt werden
        List<string> einkaufsliste = new List<string>();
        einkaufsliste.Add("Milch");
        einkaufsliste.Add("Brot");
        einkaufsliste.Add("Butter");
        einkaufsliste.Remove("Brot");
        int anzahlEinkaufsliste = einkaufsliste.Count;

        Console.WriteLine(ersteFarbe);
        Console.WriteLine(anzahlFarben);
        Console.WriteLine(anzahlEinkaufsliste);

        foreach (string artikel in einkaufsliste)
        {
            Console.WriteLine(artikel);
        }
    }
}
