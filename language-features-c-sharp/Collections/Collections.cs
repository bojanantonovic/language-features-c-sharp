using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp.Collections;

internal static class Collections
{
    public static void Show()
    {
        // Collection expression ([...]): modern, more compact alternative to "new List<int> { ... }".
        List<int> numbers = [5, 12, 3, 8, 21, 4];

        // Where: filters elements that satisfy a condition
        List<int> evenNumbers = numbers.Where(number => number % 2 == 0).ToList();

        // Select: transforms each element
        List<int> doubled = numbers.Select(number => number * 2).ToList();

        // OrderBy: sorts ascending, without changing the original list
        List<int> sorted = numbers.OrderBy(number => number).ToList();

        // Aggregate functions return a single value directly instead of a list
        int sum = numbers.Sum();
        double average = numbers.Average();
        int largestNumber = numbers.Max();

        Console.WriteLine(string.Join(", ", evenNumbers));
        Console.WriteLine(string.Join(", ", doubled));
        Console.WriteLine(string.Join(", ", sorted));
        Console.WriteLine(sum);
        Console.WriteLine(average);
        Console.WriteLine(largestNumber);
    }
}
