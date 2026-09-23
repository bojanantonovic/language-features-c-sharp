using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LanguageFeaturesCSharp.Immutable;

internal static class ImmutableCollections
{
    public static void Show()
    {
        // IReadOnlyList<T> is only a read-only VIEW on a mutable list (see CollectionInterfaces.cs):
        // whoever still holds the List<T> can change it, and the view changes along with it.
        List<string> mutableSource = ["Milk", "Bread"];
        IReadOnlyList<string> viewOnMutableList = mutableSource;
        mutableSource.Add("Butter");
        int countSeenThroughView = viewOnMutableList.Count;

        Console.WriteLine(countSeenThroughView);

        // ImmutableList<T> is a real guarantee instead of a view: nobody can change it,
        // not even the code that created it. Java's List.of() has the same intent, but throws
        // UnsupportedOperationException on add(); here Add() means something different - see below.
        ImmutableList<string> shoppingList = ["Milk", "Bread"];

        // Add changes nothing: it returns a NEW list and leaves the original untouched.
        // Because nothing can ever change, both lists can safely share their internal nodes -
        // Add copies only the path to the new element, not the whole list (a "persistent data structure").
        ImmutableList<string> extendedShoppingList = shoppingList.Add("Butter");

        int originalCount = shoppingList.Count;
        int extendedCount = extendedShoppingList.Count;

        Console.WriteLine(originalCount);
        Console.WriteLine(extendedCount);

        // Forgetting the return value is the typical beginner mistake: the call is not an error,
        // it simply has no visible effect, because the new list is thrown away immediately.
        shoppingList.Add("Cheese");
        Console.WriteLine(shoppingList.Count);

        // ImmutableArray<T> is a struct wrapping a plain array: the fastest to read and to iterate,
        // but every change copies the whole array - for data that is written once and read often.
        ImmutableArray<int> measurements = [3, 7, 2];
        ImmutableArray<int> measurementsWithExtra = measurements.Add(9);

        Console.WriteLine(string.Join(", ", measurements));
        Console.WriteLine(string.Join(", ", measurementsWithExtra));

        // Dictionary and set work the same way: Add/SetItem/Remove all return a new instance.
        // Empty is the starting point, there is no public constructor.
        ImmutableDictionary<string, int> ageByName = ImmutableDictionary<string, int>.Empty
            .Add("Alice", 30)
            .Add("Bob", 25);
        ImmutableDictionary<string, int> ageByNameWithCarol = ageByName.SetItem("Carol", 41);

        Console.WriteLine(ageByName.Count);
        Console.WriteLine(ageByNameWithCarol.Count);

        ImmutableHashSet<string> colours = ["Red", "Green"];
        ImmutableHashSet<string> moreColours = colours.Add("Blue");

        Console.WriteLine(colours.Count);
        Console.WriteLine(moreColours.Count);

        // Builder: creating one new instance per step is wasteful for many changes in a row.
        // The builder collects them in a mutable buffer and freezes the result once at the end.
        ImmutableList<int>.Builder squareBuilder = ImmutableList.CreateBuilder<int>();

        for (int number = 1; number <= 5; number++)
        {
            squareBuilder.Add(number * number);
        }

        ImmutableList<int> squares = squareBuilder.ToImmutable();

        Console.WriteLine(string.Join(", ", squares));

        // LINQ works on immutable collections like on any other IEnumerable<T>;
        // ToImmutableList() is the immutable counterpart of ToList().
        ImmutableList<string> longColours = moreColours.Where(colour => colour.Length > 3).ToImmutableList();

        Console.WriteLine(string.Join(", ", longColours.Sort()));

        // FrozenDictionary (since .NET 8) is immutable AND optimized for lookups: building it is
        // expensive, reading it is faster than Dictionary<K, V> - for lookup tables that are
        // built once at startup and then only read, e.g. from many threads at the same time.
        FrozenDictionary<string, int> portByProtocol = new Dictionary<string, int>
        {
            ["http"] = 80,
            ["https"] = 443
        }.ToFrozenDictionary();

        int httpsPort = portByProtocol["https"];

        Console.WriteLine(httpsPort);

        // Immutable collections need no lock: a value that cannot change cannot be seen half-updated.
        // The same idea for single objects instead of collections is the record (see Records.cs),
        // where the with expression plays the role of Add here.
        ImmutableList<string> sharedList = extendedShoppingList;
        string joinedItems = string.Join(", ", sharedList);

        Console.WriteLine(joinedItems);
    }
}
