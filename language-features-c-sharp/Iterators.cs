namespace LanguageFeaturesCSharp;

internal static class Iterators
{
    public static void Show()
    {
        // yield return produces the values one at a time, only when they are actually requested -
        // the method itself does not run to completion immediately.
        foreach (int number in EvenNumbersUpTo(10))
        {
            Console.WriteLine(number);
        }

        // The result of an iterator can be processed with LINQ like any other IEnumerable<T>
        List<int> evenNumbers = EvenNumbersUpTo(10).ToList();
        int sum = evenNumbers.Sum();

        Console.WriteLine(sum);

        // Custom iterator over a list: returns the elements in reverse order.
        // Collection expression ([...]): modern, more compact alternative to "new List<string> { ... }".
        List<string> names = ["Alice", "Bob", "Carol"];

        foreach (string name in Reversed(names))
        {
            Console.WriteLine(name);
        }
    }

    // IEnumerable<T> as return type + yield return: the caller gets the values one by one,
    // without a complete list having to be built in memory first.
    private static IEnumerable<int> EvenNumbersUpTo(int upperBound)
    {
        for (int number = 0; number <= upperBound; number++)
        {
            if (number % 2 == 0)
            {
                yield return number;
            }
        }
    }

    private static IEnumerable<string> Reversed(List<string> values)
    {
        for (int index = values.Count - 1; index >= 0; index--)
        {
            yield return values[index];
        }
    }
}
