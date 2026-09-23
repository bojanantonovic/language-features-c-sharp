using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp.Classes;

// struct: value type - copied on assignment, unlike a class (reference type),
// where two variables would share the same instance.
internal struct Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

internal static class Structs
{
    public static void Show()
    {
        Point pointA = new Point(1, 2);
        Point pointB = pointA; // creates a copy, not the same instance

        pointB.X = 99;

        int pointAX = pointA.X; // stays unchanged, because pointB is its own copy
        int pointBX = pointB.X;

        Console.WriteLine(pointAX);
        Console.WriteLine(pointBX);

        // List of structs: Select computes a new value from each point, the original list stays unchanged.
        // Collection expression ([...]): modern, more compact alternative to "new List<Point> { ... }".
        List<Point> points =
        [
            new Point(1, 2),
            new Point(-3, 4),
            new Point(5, -1),
        ];

        List<double> distancesFromOrigin = points
            .Select(p => Math.Sqrt((p.X * p.X) + (p.Y * p.Y)))
            .ToList();

        Point closestPoint = points
            .OrderBy(p => Math.Sqrt((p.X * p.X) + (p.Y * p.Y)))
            .First();

        Console.WriteLine(string.Join(", ", distancesFromOrigin));
        Console.WriteLine($"{closestPoint.X}, {closestPoint.Y}");
    }
}
