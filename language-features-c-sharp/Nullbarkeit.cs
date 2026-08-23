namespace LanguageFeaturesCSharp;

internal static class Nullbarkeit
{
    public static void Zeigen()
    {
        // Nullable value type: int? kann zusaetzlich zu allen int-Werten auch null sein
        int? alter = null;

        // HasValue/Value: sicher pruefen, ob ein Wert vorhanden ist, bevor er gelesen wird
        bool hatWert = alter.HasValue;

        Console.WriteLine(hatWert);

        // ??-Operator: liefert den rechten Wert, wenn der linke null ist
        int alterOderStandard = alter ?? 18;

        Console.WriteLine(alterOderStandard);

        // ??=-Operator: weist nur zu, wenn die Variable aktuell null ist
        alter ??= 21;

        Console.WriteLine(alter);

        // Nullable Reference Type: string? macht sichtbar, dass diese Variable null sein darf
        string? name = null;

        // ?.-Operator (null-conditional): ruft Length nur auf, wenn name nicht null ist,
        // sonst liefert der ganze Ausdruck direkt null.
        int? nameLaenge = name?.Length;

        Console.WriteLine(nameLaenge);

        name = "Alice";
        nameLaenge = name?.Length;

        Console.WriteLine(nameLaenge);

        // Liste mit moeglichen Luecken: LINQ filtert die null-Eintraege heraus
        List<int?> zahlenMitLuecken = new List<int?> { 5, null, 12, null, 8 };

        List<int> vorhandeneZahlen = zahlenMitLuecken
            .Where(zahl => zahl.HasValue)
            .Select(zahl => zahl!.Value)
            .ToList();

        Console.WriteLine(string.Join(", ", vorhandeneZahlen));
    }
}
