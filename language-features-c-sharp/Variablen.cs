namespace Language_Features_C_Sharp;

internal static class Variablen
{
    public static void Zeigen()
    {
        // Ganzzahl
        int alter = 30;

        // Kommazahl
        double koerpergroesse = 1.78;

        // Text
        string name = "Alice";

        // Wahrheitswert: true oder false
        bool istAktiv = true;

        // Eine Berechnung: das Ergebnis landet zuerst in einer eigenen Variable,
        // nicht direkt in der Console.WriteLine-Zeile.
        int alterInZehnJahren = alter + 10;

        Console.WriteLine(name);
        Console.WriteLine(alter);
        Console.WriteLine(koerpergroesse);
        Console.WriteLine(istAktiv);
        Console.WriteLine(alterInZehnJahren);
    }
}
