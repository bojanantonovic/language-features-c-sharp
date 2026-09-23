using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp.Collections;

internal static class Dictionaries
{
    public static void Show()
    {
        // Dictionary: stores values under a unique key
        Dictionary<string, int> ageByName = new();
        ageByName["Alice"] = 30;
        ageByName["Bob"] = 25;
        ageByName.Add("Carol", 40);

        // Access via the key
        int aliceAge = ageByName["Alice"];

        // Safe access: TryGetValue returns true/false instead of throwing an exception
        // when the key is missing
        bool found = ageByName.TryGetValue("Dave", out int daveAge);

        int entryCount = ageByName.Count;

        Console.WriteLine(aliceAge);
        Console.WriteLine(found);
        Console.WriteLine(daveAge);
        Console.WriteLine(entryCount);

        foreach (KeyValuePair<string, int> entry in ageByName)
        {
            Console.WriteLine(entry.Key);
            Console.WriteLine(entry.Value);
        }

        // LINQ on a dictionary: iterates over KeyValuePair<string, int> entries
        List<string> namesOver28 = ageByName
            .Where(entry => entry.Value > 28)
            .Select(entry => entry.Key)
            .ToList();

        // OrderByDescending + First: find the name with the highest age
        string oldestName = ageByName
            .OrderByDescending(entry => entry.Value)
            .First()
            .Key;

        Console.WriteLine(string.Join(", ", namesOver28));
        Console.WriteLine(oldestName);
    }
}
