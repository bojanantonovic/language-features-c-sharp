namespace Language_Features_C_Sharp;

internal static class Async
{
    public static async Task ZeigenAsync()
    {
        int ergebnis = await BerechneAsync(6, 7);
        Console.WriteLine(ergebnis);
    }

    // async Methode: kann mit "await" auf eine asynchrone Operation warten,
    // ohne dabei den Thread zu blockieren. Task<T> ist der "Behaelter" fuer das spaetere Ergebnis.
    private static async Task<int> BerechneAsync(int a, int b)
    {
        await Task.Delay(100); // simuliert eine dauernde Operation, z. B. einen Netzwerkaufruf
        return a * b;
    }
}
