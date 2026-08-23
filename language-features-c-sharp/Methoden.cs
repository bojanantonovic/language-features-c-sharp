namespace Language_Features_C_Sharp;

internal static class Methoden
{
    public static void Zeigen()
    {
        int quadrat = Quadrieren(6);
        int summeZweierZahlen = Addieren(3, 4);
        int summeBis10 = SummeVon1Bis(10);

        Console.WriteLine(quadrat);
        Console.WriteLine(summeZweierZahlen);
        Console.WriteLine(summeBis10);
    }

    // Methode mit einem Parameter und Rueckgabewert
    private static int Quadrieren(int zahl)
    {
        return zahl * zahl;
    }

    // Methode mit zwei Parametern und Rueckgabewert
    private static int Addieren(int a, int b)
    {
        return a + b;
    }

    // Methode mit Parameter und Rueckgabewert, die intern eine Schleife nutzt
    // (macht die for-Schleife aus Schleifen.cs fuer beliebige Obergrenzen wiederverwendbar)
    private static int SummeVon1Bis(int grenze)
    {
        int summe = 0;
        for (int i = 1; i <= grenze; i++)
        {
            summe = summe + i;
        }

        return summe;
    }
}
