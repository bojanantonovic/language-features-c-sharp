using System.Reflection;

namespace LanguageFeaturesCSharp;

// Eigenes Attribut: erbt von Attribute, kann anschliessend auf Klassen oder Methoden angewendet werden
[AttributeUsage(AttributeTargets.Method)]
internal class AutorAttribute : Attribute
{
    public string Name { get; }

    public AutorAttribute(string name)
    {
        Name = name;
    }
}

internal static class Attribut
{
    [Autor("Alice")]
    public static void Zeigen()
    {
        // Reflection: das Attribut wird zur Laufzeit von der Methode ausgelesen,
        // nicht schon beim Kompilieren verwendet.
        MethodInfo methode = typeof(Attribut).GetMethod(nameof(Zeigen))!;
        AutorAttribute? attribut = methode.GetCustomAttribute<AutorAttribute>();

        string autorName = attribut?.Name ?? "unbekannt";

        Console.WriteLine(autorName);

        // Reflection liefert alle Methoden der Klasse, LINQ filtert daraus die mit einem Autor-Attribut
        MethodInfo[] alleMethoden = typeof(Attribut).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        List<string> autorenProMethode = alleMethoden
            .Where(m => m.GetCustomAttribute<AutorAttribute>() is not null)
            .Select(m => $"{m.Name}: {m.GetCustomAttribute<AutorAttribute>()!.Name}")
            .ToList();

        Console.WriteLine(string.Join(" | ", autorenProMethode));
    }

    [Autor("Bob")]
    private static void Hilfsmethode()
    {
    }
}
