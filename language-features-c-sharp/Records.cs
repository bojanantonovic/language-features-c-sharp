namespace Language_Features_C_Sharp;

// record: kompakte Syntax fuer einen unveraenderlichen Datentyp mit automatischer Werte-Gleichheit
internal record Adresse(string Strasse, string Stadt);

internal static class Records
{
    public static void Zeigen()
    {
        Adresse adresse1 = new Adresse("Bahnhofstrasse 1", "Zuerich");
        Adresse adresse2 = new Adresse("Bahnhofstrasse 1", "Zuerich");

        // records vergleichen ihre Werte, nicht die Referenz (anders als eine normale Klasse)
        bool sindGleich = adresse1 == adresse2;

        // with-Expression: erzeugt eine neue Kopie mit einer geaenderten Eigenschaft,
        // das urspruengliche Objekt bleibt dabei unveraendert
        Adresse adresse3 = adresse1 with { Stadt = "Bern" };

        Console.WriteLine(sindGleich);
        Console.WriteLine(adresse1.Stadt);
        Console.WriteLine(adresse3.Stadt);
        Console.WriteLine(adresse1);

        // Liste von records: LINQ funktioniert auf ihnen genauso wie in Collections.cs auf int
        List<Adresse> adressen = new List<Adresse>
        {
            new Adresse("Bahnhofstrasse 1", "Zuerich"),
            new Adresse("Marktgasse 5", "Bern"),
            new Adresse("Seestrasse 12", "Zuerich"),
        };

        // Where + Select: erst filtern, dann nur die Strasse aus dem record herausziehen
        List<string> zuercherStrassen = adressen
            .Where(adresse => adresse.Stadt == "Zuerich")
            .Select(adresse => adresse.Strasse)
            .ToList();

        // GroupBy: gruppiert die Adressen anhand einer Eigenschaft des records
        var adressenProStadt = adressen.GroupBy(adresse => adresse.Stadt);

        Console.WriteLine(string.Join(", ", zuercherStrassen));

        foreach (var gruppe in adressenProStadt)
        {
            Console.WriteLine($"{gruppe.Key}: {gruppe.Count()}");
        }
    }
}
