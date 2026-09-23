namespace LanguageFeaturesCSharp.Classes;

// struct with overloaded operators: defines what "+" and "==" mean for this custom type
internal struct Vector2Int
{
    public int X { get; }

    public int Y { get; }

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    // operator +: defines how two Vector2Int values are added
    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    // operator ==/!=: custom equality check based on the values instead of the reference
    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2Int otherVector && this == otherVector;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

internal static class Operators
{
    public static void Show()
    {
        Vector2Int a = new Vector2Int(1, 2);
        Vector2Int b = new Vector2Int(3, 4);

        // calls operator + even though Vector2Int is a hand-written struct
        Vector2Int sum = a + b;

        // calls operator ==: compares X and Y, not the reference
        bool areEqual = a == b;
        bool areNotEqual = a != b;

        Console.WriteLine(sum);
        Console.WriteLine(areEqual);
        Console.WriteLine(areNotEqual);

        // List of vectors: Aggregate repeatedly applies operator + to add them all up.
        // Collection expression ([...]): modern, more compact alternative to "new List<Vector2Int> { ... }".
        List<Vector2Int> vectors = [new Vector2Int(1, 1), new Vector2Int(2, 3), new Vector2Int(-1, 4)];

        Vector2Int totalSum = vectors.Aggregate((v1, v2) => v1 + v2);

        Console.WriteLine(totalSum);
    }
}
