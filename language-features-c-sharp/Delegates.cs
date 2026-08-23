namespace Language_Features_C_Sharp;

// Eigener Delegate-Typ: beschreibt nur die Signatur einer Methode (Parameter + Rueckgabewert)
internal delegate int RechenOperation(int a, int b);

// Klasse mit einem Event: benachrichtigt andere Codeteile, wenn sich etwas aendert
internal class Konto
{
    public decimal Guthaben { get; private set; }

    // Event basiert auf einem Delegate (hier: Action<decimal>)
    public event Action<decimal>? GuthabenGeaendert;

    public void Einzahlen(decimal betrag)
    {
        Guthaben += betrag;
        GuthabenGeaendert?.Invoke(Guthaben); // loest das Event aus, falls jemand zuhoert
    }
}

internal static class Delegates
{
    public static void Zeigen()
    {
        // Delegate: einer Methode zuweisen und wie eine Variable aufrufen
        RechenOperation addieren = Addieren;
        int summe = addieren(3, 4);

        // Delegate: einem Lambda-Ausdruck zuweisen
        RechenOperation multiplizieren = (a, b) => a * b;
        int produkt = multiplizieren(3, 4);

        Console.WriteLine(summe);
        Console.WriteLine(produkt);

        // Liste von Delegates: LINQ kann jeden einzelnen auf dieselben Argumente anwenden
        List<RechenOperation> operationen = new List<RechenOperation>
        {
            Addieren,
            (a, b) => a * b,
            (a, b) => a - b,
        };

        List<int> ergebnisse = operationen.Select(operation => operation(10, 3)).ToList();

        Console.WriteLine(string.Join(", ", ergebnisse));

        // Event abonnieren: die Lambda wird aufgerufen, sobald GuthabenGeaendert ausgeloest wird
        Konto konto = new Konto();
        konto.GuthabenGeaendert += neuesGuthaben => Console.WriteLine(neuesGuthaben);

        konto.Einzahlen(100);
        konto.Einzahlen(50);
    }

    private static int Addieren(int a, int b)
    {
        return a + b;
    }
}
