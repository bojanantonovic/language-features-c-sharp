using System;
using System.Collections;
using System.Collections.Generic;

namespace LanguageFeaturesCSharp.Loops;

// IEnumerable<T> means "can be iterated" and is the counterpart of Java's Iterable<T>.
// The collection itself holds only the data, not a position.
internal class Countdown : IEnumerable<int>
{
    private readonly int start;

    public Countdown(int start)
    {
        this.start = start;
    }

    // foreach calls this method once, to get a fresh cursor over the data
    public IEnumerator<int> GetEnumerator()
    {
        return new CountdownEnumerator(start);
    }

    // The non-generic version is inherited from IEnumerable and exists only for code from
    // before generics. It is implemented explicitly, so it does not clutter the type itself.
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

// IEnumerator<T> is the cursor: it knows the position, and several cursors can run over
// the same collection independently (Java: Iterator<T>).
internal class CountdownEnumerator : IEnumerator<int>
{
    private readonly int start;

    private int current;

    public CountdownEnumerator(int start)
    {
        this.start = start;
        current = start + 1; // one step before the first element
    }

    // Current returns the element at the current position without moving on;
    // it may be read only after MoveNext has returned true.
    public int Current => current;

    // The non-generic Current boxes the value into an object - again only for old code
    object IEnumerator.Current => Current;

    // MoveNext does two things at once: move on, and report whether an element is there.
    // Java splits this in two: hasNext() only asks, next() only moves on and returns.
    public bool MoveNext()
    {
        current--;

        return current >= 1;
    }

    // Part of the interface, but rarely implemented usefully - most enumerators throw here
    public void Reset()
    {
        current = start + 1;
    }

    // IEnumerator<T> is IDisposable: a cursor over a file or a database connection releases
    // it here, and foreach guarantees the call. Java's Iterator has no such hook.
    public void Dispose()
    {
    }
}

internal static class Enumerators
{
    public static void Show()
    {
        // The custom collection is used exactly like a built-in one
        Countdown countdown = new Countdown(3);

        foreach (int number in countdown)
        {
            Console.WriteLine(number);
        }

        // ... because foreach is only shorthand for this: fetch a cursor, move it forward
        // as long as there are elements, and dispose of it at the end.
        IEnumerator<int> enumerator = countdown.GetEnumerator();

        try
        {
            while (enumerator.MoveNext())
            {
                int number = enumerator.Current;

                Console.WriteLine(number);
            }
        }
        finally
        {
            enumerator.Dispose();
        }

        // The same protocol works on every collection, because List<T> implements it too
        List<string> names = ["Alice", "Bob", "Carol"];
        IEnumerator<string> nameEnumerator = names.GetEnumerator();
        nameEnumerator.MoveNext();
        string firstName = nameEnumerator.Current;
        nameEnumerator.MoveNext();
        string secondName = nameEnumerator.Current;
        nameEnumerator.Dispose();

        Console.WriteLine(firstName);
        Console.WriteLine(secondName);

        // Two cursors over the same collection have their own position each - which is why
        // a nested foreach over one list works at all.
        IEnumerator<int> firstCursor = countdown.GetEnumerator();
        IEnumerator<int> secondCursor = countdown.GetEnumerator();
        firstCursor.MoveNext();
        firstCursor.MoveNext();
        secondCursor.MoveNext();

        Console.WriteLine(firstCursor.Current);
        Console.WriteLine(secondCursor.Current);

        // Reset puts the cursor back in front of the first element
        firstCursor.Reset();
        firstCursor.MoveNext();

        Console.WriteLine(firstCursor.Current);

        firstCursor.Dispose();
        secondCursor.Dispose();

        // The cursor of a built-in collection notices changes to the data and refuses to
        // continue - the counterpart of Java's ConcurrentModificationException.
        try
        {
            foreach (string name in names)
            {
                names.Add(name);
            }
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine(exception.Message);
        }

        // A method that only needs to be iterated takes IEnumerable<T>: the Countdown, a list
        // and an iterator method all fit, because they all provide the same cursor.
        Console.WriteLine(SumOf(countdown));
        Console.WriteLine(SumOf([10, 20, 30]));
        Console.WriteLine(SumOf(CountdownWithYield(3)));
    }

    // Implementing the whole protocol by hand is rarely necessary: yield return builds the
    // same enumerator class automatically, as a state machine (see Iterators.cs).
    private static IEnumerable<int> CountdownWithYield(int start)
    {
        for (int number = start; number >= 1; number--)
        {
            yield return number;
        }
    }

    // IEnumerable<T> is the most general collection parameter - it says nothing more than
    // "this can be walked over once, element by element".
    private static int SumOf(IEnumerable<int> numbers)
    {
        int total = 0;

        foreach (int number in numbers)
        {
            total += number;
        }

        return total;
    }
}
