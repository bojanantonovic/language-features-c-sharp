namespace LanguageFeaturesCSharp;

// Eigener Typ mit Eigenschaften (Properties): Name und Alter koennen gelesen und geschrieben werden.
internal class Person
{
    public string Name { get; set; } = string.Empty;

    public int Alter { get; set; }
}

internal static class Klassen
{
    public static void Zeigen()
    {
        // Objekt erzeugen und Eigenschaften einzeln setzen
        Person alice = new Person();
        alice.Name = "Alice";
        alice.Alter = 30;

        // Eigenschaften auslesen, Ergebnis landet in einer eigenen Variable
        string aliceName = alice.Name;
        int aliceAlter = alice.Alter;

        // Objektinitialisierer: Eigenschaften direkt beim Erzeugen setzen
        Person bob = new Person { Name = "Bob", Alter = 25 };

        Console.WriteLine(aliceName);
        Console.WriteLine(aliceAlter);
        Console.WriteLine(bob.Name);
        Console.WriteLine(bob.Alter);

        // Liste von Objekten: LINQ funktioniert auch auf eigenen Klassen, nicht nur auf primitiven Typen
        List<Person> personen = new List<Person>
        {
            alice,
            bob,
            new Person { Name = "Carol", Alter = 40 },
        };

        // OrderByDescending + First: die Person mit dem hoechsten Alter finden
        Person aelteste = personen.OrderByDescending(person => person.Alter).First();

        // Average berechnet aus einer aus den Objekten abgeleiteten Zahl
        double durchschnittsalter = personen.Average(person => person.Alter);

        Console.WriteLine(aelteste.Name);
        Console.WriteLine(durchschnittsalter);
    }
}
