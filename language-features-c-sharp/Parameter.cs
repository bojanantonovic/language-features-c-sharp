namespace Language_Features_C_Sharp;

internal static class Parameter
{
    public static void Zeigen()
    {
        // out: die Methode MUSS diesem Parameter einen Wert zuweisen, gedacht fuer zusaetzliche Rueckgabewerte
        bool erfolg = TryDividieren(10, 2, out int ergebnis);

        Console.WriteLine(erfolg);
        Console.WriteLine(ergebnis);

        // ref: die Methode kann den bestehenden Wert der Variablen lesen UND aendern
        int zahl = 5;
        Verdoppeln(ref zahl);

        Console.WriteLine(zahl);

        // in: die Methode bekommt den Wert nur zum Lesen, eine Kopie wird dabei vermieden
        Vektor a = new Vektor(3, 4);
        double laenge = Laenge(in a);

        Console.WriteLine(laenge);

        // Optionaler Parameter: rabatt hat einen Standardwert, muss also nicht angegeben werden
        double preisOhneRabatt = PreisBerechnen(100);
        double preisMitRabatt = PreisBerechnen(100, 0.1);

        Console.WriteLine(preisOhneRabatt);
        Console.WriteLine(preisMitRabatt);

        // params: beliebig viele Argumente werden von der Methode als Array entgegengenommen
        int summe = Summiere(1, 2, 3, 4, 5);

        Console.WriteLine(summe);
    }

    private static bool TryDividieren(int zahler, int nenner, out int ergebnis)
    {
        if (nenner == 0)
        {
            ergebnis = 0;
            return false;
        }

        ergebnis = zahler / nenner;
        return true;
    }

    private static void Verdoppeln(ref int wert)
    {
        wert *= 2;
    }

    private static double Laenge(in Vektor vektor)
    {
        return Math.Sqrt((vektor.X * vektor.X) + (vektor.Y * vektor.Y));
    }

    private static double PreisBerechnen(double preis, double rabatt = 0.0)
    {
        return preis - (preis * rabatt);
    }

    private static int Summiere(params int[] zahlen)
    {
        return zahlen.Sum();
    }
}
