namespace LanguageFeaturesCSharp;

// Interface: legt nur fest, WAS eine Klasse koennen muss, nicht WIE (keine gemeinsame Basisklasse noetig)
internal interface IFahrzeug
{
    string Bewegen();
}

internal class Auto : IFahrzeug
{
    public string Bewegen()
    {
        return "faehrt auf der Strasse";
    }
}

internal class Fahrrad : IFahrzeug
{
    public string Bewegen()
    {
        return "faehrt auf dem Radweg";
    }
}

internal static class Interfaces
{
    public static void Zeigen()
    {
        // Liste vom Interface-Typ: Auto und Fahrrad haben keine gemeinsame Basisklasse,
        // erfuellen aber beide den Vertrag von IFahrzeug.
        List<IFahrzeug> fahrzeuge = new List<IFahrzeug>();
        fahrzeuge.Add(new Auto());
        fahrzeuge.Add(new Fahrrad());

        foreach (IFahrzeug fahrzeug in fahrzeuge)
        {
            string bewegung = fahrzeug.Bewegen();
            Console.WriteLine(bewegung);
        }

        // Select: wandelt jedes Fahrzeug (ueber das Interface) in seinen Bewegungstext um
        List<string> bewegungen = fahrzeuge.Select(fahrzeug => fahrzeug.Bewegen()).ToList();

        // OfType<T>: filtert aus der Interface-Liste gezielt nur die Autos heraus
        int anzahlAutos = fahrzeuge.OfType<Auto>().Count();

        Console.WriteLine(string.Join(" / ", bewegungen));
        Console.WriteLine(anzahlAutos);
    }
}
