namespace LanguageFeaturesCSharp;

internal record Kreis(double Radius);

internal record Rechteck(double Breite, double Hoehe);

internal record Quadrat(double Seitenlaenge);

// Versand enthaelt ein anderes record als Inhalt: die Basis fuer ein Nested Pattern weiter unten
internal record Versand(object Inhalt, bool Prioritaet);

internal static class RecordsMitMusterAbgleich
{
    public static void Zeigen()
    {
        object formA = new Kreis(5);
        object formB = new Rechteck(4, 2);
        object formC = new Quadrat(3);

        // Positional Pattern: zerlegt jedes record direkt in seine Positionsparameter
        Console.WriteLine(Beschreiben(formA));
        Console.WriteLine(Beschreiben(formB));
        Console.WriteLine(Beschreiben(formC));

        // when-Guard: dasselbe Pattern (Kreis), aber mit zusaetzlicher Bedingung an den entpackten Wert
        Console.WriteLine(Beschreiben(new Kreis(9)));

        // Nested Pattern: Versand(Rechteck(...), true) prueft in einem Ausdruck sowohl den Typ von
        // Versand als auch den Typ und die Werte des darin enthaltenen records.
        Versand eiligeSendung = new Versand(new Rechteck(2, 2), Prioritaet: true);
        Versand normaleSendung = new Versand(new Kreis(1), Prioritaet: false);

        Console.WriteLine(Beschreiben(eiligeSendung));
        Console.WriteLine(Beschreiben(normaleSendung));

        // Property Pattern kombiniert mit Relational Pattern: { Breite: > 0, Hoehe: > 0 } prueft
        // beide Eigenschaften direkt im Pattern, ohne sie vorher einzeln auszulesen.
        double flaecheGueltig = Flaeche(new Rechteck(10, 2));
        double flaecheUngueltig = Flaeche(new Rechteck(0, 5));

        Console.WriteLine(flaecheGueltig);
        Console.WriteLine(flaecheUngueltig);

        // Liste gemischter Formen: LINQ mit is-Pattern in der Lambda filtert nach dem tatsaechlichen record-Typ
        List<object> formen = new List<object> { new Kreis(1), new Rechteck(2, 3), new Quadrat(4), new Kreis(10) };

        int anzahlKreise = formen.Count(form => form is Kreis);
        List<double> kreisRadien = formen.OfType<Kreis>().Select(kreis => kreis.Radius).ToList();

        Console.WriteLine(anzahlKreise);
        Console.WriteLine(string.Join(", ", kreisRadien));
    }

    // switch-Expression mit Positional Patterns: "form switch { Kreis(var radius) => ... }" prueft
    // Typ UND Struktur in einem Schritt, "when" ergaenzt eine zusaetzliche Bedingung an den Wert.
    private static string Beschreiben(object form)
    {
        return form switch
        {
            Kreis(var radius) when radius > 5 => $"Grosser Kreis mit Radius {radius}",
            Kreis(var radius) => $"Kreis mit Radius {radius}",
            Quadrat(var seite) => $"Quadrat mit Seitenlaenge {seite}",
            Rechteck(var breite, var hoehe) => $"Rechteck {breite} x {hoehe}",
            Versand(Rechteck(var breite, var hoehe), true) => $"Prioritaetsversand: Rechteck {breite} x {hoehe}",
            Versand(var inhalt, false) => $"Normaler Versand: {Beschreiben(inhalt)}",
            _ => "Unbekannte Form"
        };
    }

    private static double Flaeche(Rechteck rechteck)
    {
        return rechteck switch
        {
            { Breite: > 0, Hoehe: > 0 } => rechteck.Breite * rechteck.Hoehe,
            _ => 0
        };
    }
}
