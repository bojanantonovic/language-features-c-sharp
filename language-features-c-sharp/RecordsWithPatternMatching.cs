using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageFeaturesCSharp;

internal record Circle(double Radius);

internal record Rectangle(double Width, double Height);

internal record Square(double SideLength);

// Shipment holds another record as its content: the basis for a nested pattern further below
internal record Shipment(object Content, bool Priority);

internal static class RecordsWithPatternMatching
{
    public static void Show()
    {
        object shapeA = new Circle(5);
        object shapeB = new Rectangle(4, 2);
        object shapeC = new Square(3);

        // Positional pattern: deconstructs each record directly into its positional parameters
        Console.WriteLine(Describe(shapeA));
        Console.WriteLine(Describe(shapeB));
        Console.WriteLine(Describe(shapeC));

        // when guard: the same pattern (Circle), but with an extra condition on the unpacked value
        Console.WriteLine(Describe(new Circle(9)));

        // Nested pattern: Shipment(Rectangle(...), true) checks in one expression both the type of
        // Shipment and the type and values of the record it contains.
        Shipment urgentShipment = new Shipment(new Rectangle(2, 2), Priority: true);
        Shipment normalShipment = new Shipment(new Circle(1), Priority: false);

        Console.WriteLine(Describe(urgentShipment));
        Console.WriteLine(Describe(normalShipment));

        // Property pattern combined with a relational pattern: { Width: > 0, Height: > 0 } checks
        // both properties directly in the pattern, without reading them individually beforehand.
        double validArea = Area(new Rectangle(10, 2));
        double invalidArea = Area(new Rectangle(0, 5));

        Console.WriteLine(validArea);
        Console.WriteLine(invalidArea);

        // List of mixed shapes: LINQ with an is pattern in the lambda filters by the actual record type.
        // Collection expression ([...]): modern, more compact alternative to "new List<object> { ... }".
        List<object> shapes = [new Circle(1), new Rectangle(2, 3), new Square(4), new Circle(10)];

        int circleCount = shapes.Count(shape => shape is Circle);
        List<double> circleRadii = shapes.OfType<Circle>().Select(circle => circle.Radius).ToList();

        Console.WriteLine(circleCount);
        Console.WriteLine(string.Join(", ", circleRadii));
    }

    // switch expression with positional patterns: "shape switch { Circle(var radius) => ... }" checks
    // type AND structure in one step, "when" adds an extra condition on the value.
    private static string Describe(object shape)
    {
        return shape switch
        {
            Circle(var radius) when radius > 5 => $"Large circle with radius {radius}",
            Circle(var radius) => $"Circle with radius {radius}",
            Square(var side) => $"Square with side length {side}",
            Rectangle(var width, var height) => $"Rectangle {width} x {height}",
            Shipment(Rectangle(var width, var height), true) => $"Priority shipment: rectangle {width} x {height}",
            Shipment(var content, false) => $"Normal shipment: {Describe(content)}",
            _ => "Unknown shape"
        };
    }

    private static double Area(Rectangle rectangle)
    {
        return rectangle switch
        {
            { Width: > 0, Height: > 0 } => rectangle.Width * rectangle.Height,
            _ => 0
        };
    }
}
