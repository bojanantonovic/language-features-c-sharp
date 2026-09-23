using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp;

internal static class Nullability
{
    public static void Show()
    {
        // Nullable value type: int? can be null in addition to all int values
        int? age = null;

        // HasValue/Value: safely check whether a value is present before reading it
        bool hasValue = age.HasValue;

        Console.WriteLine(hasValue);

        // ?? operator: returns the right-hand value if the left-hand one is null
        int ageOrDefault = age ?? 18;

        Console.WriteLine(ageOrDefault);

        // ??= operator: only assigns if the variable is currently null
        age ??= 21;

        Console.WriteLine(age);

        // Nullable reference type: string? makes it visible that this variable may be null
        string? name = null;

        // ?. operator (null-conditional): only calls Length if name is not null,
        // otherwise the whole expression evaluates directly to null.
        int? nameLength = name?.Length;

        Console.WriteLine(nameLength);

        name = "Alice";
        nameLength = name?.Length;

        Console.WriteLine(nameLength);

        // List with possible gaps: LINQ filters out the null entries.
        // Collection expression ([...]): modern, more compact alternative to "new List<int?> { ... }".
        List<int?> numbersWithGaps = [5, null, 12, null, 8];

        List<int> presentNumbers = numbersWithGaps
            .Where(number => number.HasValue)
            .Select(number => number!.Value)
            .ToList();

        Console.WriteLine(string.Join(", ", presentNumbers));
    }
}
