namespace LanguageFeaturesCSharp;

// Eigene Exception-Klasse: erbt von Exception, fuer einen spezifischen Fehlerfall im eigenen Code
internal class UngueltigesAlterException : Exception
{
    public UngueltigesAlterException(string nachricht) : base(nachricht)
    {
    }
}

internal static class Exceptions
{
    public static void Zeigen()
    {
        int ergebnisOk = SicherDividieren(10, 2);
        int ergebnisFehler = SicherDividieren(10, 0);

        Console.WriteLine(ergebnisOk);
        Console.WriteLine(ergebnisFehler);

        try
        {
            PruefeAlter(-5);
        }
        catch (UngueltigesAlterException ex)
        {
            // faengt gezielt nur unsere eigene Exception ab, nicht jede beliebige Exception
            Console.WriteLine(ex.Message);
        }
    }

    // wirft die eigene Exception, wenn der uebergebene Wert fachlich ungueltig ist
    private static void PruefeAlter(int alter)
    {
        if (alter < 0)
        {
            throw new UngueltigesAlterException($"Alter darf nicht negativ sein: {alter}");
        }
    }

    private static int SicherDividieren(int zahler, int nenner)
    {
        try
        {
            int ergebnis = zahler / nenner;
            return ergebnis;
        }
        catch (DivideByZeroException)
        {
            // catch faengt die Exception ab, statt das Programm abstuerzen zu lassen
            Console.WriteLine("Fehler: Division durch 0 ist nicht erlaubt.");
            return 0;
        }
        finally
        {
            // finally laeuft immer, egal ob eine Exception aufgetreten ist oder nicht
            Console.WriteLine("SicherDividieren wurde aufgerufen.");
        }
    }
}
