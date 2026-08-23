using System.Reflection;

namespace LanguageFeaturesCSharp;

// Custom attribute: inherits from Attribute, can then be applied to classes or methods
[AttributeUsage(AttributeTargets.Method)]
internal class AuthorAttribute : Attribute
{
    public string Name { get; }

    public AuthorAttribute(string name)
    {
        Name = name;
    }
}

internal static class Attributes
{
    [Author("Alice")]
    public static void Show()
    {
        // Reflection: the attribute is read from the method at runtime,
        // not already used at compile time.
        MethodInfo method = typeof(Attributes).GetMethod(nameof(Show))!;
        AuthorAttribute? attribute = method.GetCustomAttribute<AuthorAttribute>();

        string authorName = attribute?.Name ?? "unknown";

        Console.WriteLine(authorName);

        // Reflection returns all methods of the class, LINQ filters out the ones with an author attribute
        MethodInfo[] allMethods = typeof(Attributes).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

        List<string> authorsByMethod = allMethods
            .Where(m => m.GetCustomAttribute<AuthorAttribute>() is not null)
            .Select(m => $"{m.Name}: {m.GetCustomAttribute<AuthorAttribute>()!.Name}")
            .ToList();

        Console.WriteLine(string.Join(" | ", authorsByMethod));
    }

    [Author("Bob")]
    private static void HelperMethod()
    {
    }
}
