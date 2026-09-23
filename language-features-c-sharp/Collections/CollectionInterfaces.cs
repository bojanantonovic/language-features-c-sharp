using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp.Collections;

internal static class CollectionInterfaces
{
    public static void Show()
    {
        // Like Java's "Map<String, Integer> m = new HashMap<>()", C# separates the contract
        // from the implementation: IDictionary<K, V> is the interface, Dictionary<K, V> one
        // implementation of it (others: SortedDictionary<K, V>, ConcurrentDictionary<K, V>).
        IDictionary<string, int> ageByName = new Dictionary<string, int>();
        ageByName.Add("Alice", 30);
        ageByName.Add("Bob", 25);

        // SortedDictionary fulfils the same interface, but keeps the keys sorted
        // (it is the counterpart of Java's TreeMap).
        IDictionary<string, int> sortedAgeByName = new SortedDictionary<string, int>();
        sortedAgeByName.Add("Bob", 25);
        sortedAgeByName.Add("Alice", 30);

        // Because both are IDictionary, the same method works for either implementation.
        string namesOfDictionary = JoinKeys(ageByName);
        string namesOfSortedDictionary = JoinKeys(sortedAgeByName);

        Console.WriteLine(namesOfDictionary);
        Console.WriteLine(namesOfSortedDictionary);

        // The same separation exists for lists: IList<T> is the contract,
        // List<T> the usual implementation (Java: List / ArrayList).
        IList<string> shoppingList = new List<string>();
        shoppingList.Add("Milk");
        shoppingList.Add("Bread");

        int itemCount = shoppingList.Count;
        Console.WriteLine(itemCount);

        // The interfaces build on each other, from the most general to the most specific:
        // IEnumerable<T> (only iterate) -> ICollection<T> (+ Count, Add, Remove) -> IList<T> (+ index access)
        IEnumerable<string> iterableOnly = shoppingList;
        ICollection<string> countableAndModifiable = shoppingList;

        // A parameter typed as IEnumerable<T> accepts a List<T>, an array, a HashSet<T>, ...
        int lengthOfList = TotalLength(shoppingList);
        int lengthOfArray = TotalLength(["Red", "Green", "Blue"]);
        int lengthOfSet = TotalLength(new HashSet<string> { "Yes", "No" });

        Console.WriteLine(lengthOfList);
        Console.WriteLine(lengthOfArray);
        Console.WriteLine(lengthOfSet);

        // Read-only interfaces have no Java equivalent: they hand out a view
        // that offers no Add/Remove at all, instead of throwing at runtime
        // like Java's Collections.unmodifiableList.
        IReadOnlyList<string> readOnlyList = (List<string>)shoppingList;
        IReadOnlyDictionary<string, int> readOnlyAges = (Dictionary<string, int>)ageByName;

        string firstItem = readOnlyList[0];
        int aliceAge = readOnlyAges["Alice"];

        Console.WriteLine(firstItem);
        Console.WriteLine(aliceAge);
        Console.WriteLine(iterableOnly.Count());
        Console.WriteLine(countableAndModifiable.Count);
    }

    // Works for every IDictionary implementation, not just for Dictionary<K, V>
    private static string JoinKeys(IDictionary<string, int> entries)
    {
        return string.Join(", ", entries.Keys);
    }

    // IEnumerable<T> is the most general collection parameter: it only needs to be iterable
    private static int TotalLength(IEnumerable<string> texts)
    {
        int total = 0;

        foreach (string text in texts)
        {
            total += text.Length;
        }

        return total;
    }
}
