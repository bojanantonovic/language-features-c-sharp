namespace Language_Features_C_Sharp;

internal static class Bedingungen
{
    public static void Zeigen()
    {
        int alter = 30;
        bool istAktiv = true;

        // if/else auf einem bool: nur zwei Moeglichkeiten
        string status;
        if (istAktiv)
        {
            status = "aktiv";
        }
        else
        {
            status = "inaktiv";
        }

        // if/else if/else: mehrere Faelle nacheinander pruefen
        string altersgruppe;
        if (alter < 18)
        {
            altersgruppe = "minderjaehrig";
        }
        else if (alter < 65)
        {
            altersgruppe = "erwachsen";
        }
        else
        {
            altersgruppe = "senior";
        }

        Console.WriteLine(status);
        Console.WriteLine(altersgruppe);
    }
}
