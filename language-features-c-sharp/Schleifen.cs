namespace Language_Features_C_Sharp;

internal static class Schleifen
{
    public static void Zeigen()
    {
        // for-Schleife: Summe der Zahlen 1 bis 5
        int summe = 0;
        for (int i = 1; i <= 5; i++)
        {
            summe = summe + i;
        }

        // while-Schleife: verdoppeln, bis ein Grenzwert erreicht ist
        int wert = 1;
        while (wert < 100)
        {
            wert = wert * 2;
        }

        // foreach-Schleife: ueber eine Liste von Zahlen iterieren
        int[] zahlen = { 3, 7, 2, 9, 4 };
        int groessteZahl = zahlen[0];
        foreach (int zahl in zahlen)
        {
            if (zahl > groessteZahl)
            {
                groessteZahl = zahl;
            }
        }

        Console.WriteLine(summe);
        Console.WriteLine(wert);
        Console.WriteLine(groessteZahl);
    }
}
