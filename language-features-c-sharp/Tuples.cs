using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp;

internal static class Tuples
{
    public static void Show()
    {
        // Tuple: return several values together without needing a dedicated class for it
        (string name, int age) person = ("Alice", 30);

        Console.WriteLine(person.name);
        Console.WriteLine(person.age);

        // Deconstruction: unpack a tuple's fields directly into their own variables
        (string firstName, int years) = person;

        Console.WriteLine(firstName);
        Console.WriteLine(years);

        // Tuple as a method return value
        (int min, int max) minMax = MinMax(new List<int> { 5, 12, 3, 8, 21, 4 });

        Console.WriteLine(minMax.min);
        Console.WriteLine(minMax.max);

        // List of tuples: LINQ works the same as with any other type.
        // Collection expression ([...]): modern, more compact alternative to "new List<...> { ... }".
        List<(string name, int age)> people =
        [
            ("Alice", 30),
            ("Bob", 25),
            ("Carol", 40),
        ];

        List<string> namesOver28 = people
            .Where(p => p.age > 28)
            .Select(p => p.name)
            .ToList();

        Console.WriteLine(string.Join(", ", namesOver28));
    }

    // Named tuple as return type: (int min, int max) instead of a dedicated class for two values
    private static (int min, int max) MinMax(List<int> numbers)
    {
        return (numbers.Min(), numbers.Max());
    }
}
