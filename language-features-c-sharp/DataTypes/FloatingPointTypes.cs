using System;

namespace LanguageFeaturesCSharp.DataTypes;

internal static class FloatingPointTypes
{
    public static void Show()
    {
        // float = Single (32 bit), double = Double (64 bit): both store binary fractions,
        // exactly like Java's float and double. The suffixes f and d pick the type of a literal.
        float temperature = 21.5f;
        double distance = 384_400.0d;

        Console.WriteLine(temperature);
        Console.WriteLine(distance);

        // decimal has no Java counterpart among the primitives: 128 bit, base 10, about 28 digits.
        // It is slower than double, but stores decimal fractions exactly (Java uses BigDecimal for this).
        decimal price = 19.99m;

        Console.WriteLine(price);

        // The classic binary-fraction surprise: 0.1 and 0.2 cannot be represented exactly,
        // so the sum is slightly off - in decimal the same calculation is exact.
        double binarySum = 0.1 + 0.2;
        bool binarySumIsExact = binarySum == 0.3;
        decimal decimalSum = 0.1m + 0.2m;
        bool decimalSumIsExact = decimalSum == 0.3m;

        Console.WriteLine(binarySum);
        Console.WriteLine(binarySumIsExact);
        Console.WriteLine(decimalSum);
        Console.WriteLine(decimalSumIsExact);

        // That is why money belongs in decimal and measurements in double:
        // added up often enough, the tiny error in double becomes visible.
        double doubleTotal = 0.0;
        decimal decimalTotal = 0.0m;

        for (int round = 0; round < 10; round++)
        {
            doubleTotal += 0.1;
            decimalTotal += 0.1m;
        }

        Console.WriteLine(doubleTotal == 1.0);
        Console.WriteLine(decimalTotal == 1.0m);

        // Comparing doubles therefore works with a tolerance instead of ==.
        double tolerance = 1e-9;
        bool closeEnough = Math.Abs(binarySum - 0.3) < tolerance;

        Console.WriteLine(closeEnough);

        // float.Epsilon is NOT such a tolerance: it is the smallest positive value
        // a float can represent at all - a common misunderstanding.
        double smallestPositiveDouble = double.Epsilon;

        Console.WriteLine(smallestPositiveDouble);

        // Division by zero is an exception for integers, but a special value for floating point.
        double positiveInfinity = 1.0 / 0.0;
        double notANumber = 0.0 / 0.0;

        Console.WriteLine(positiveInfinity);
        Console.WriteLine(notANumber);

        // NaN is not equal to anything, not even to itself - so it needs its own check.
        bool nanEqualsItself = notANumber == double.NaN;
        bool nanDetected = double.IsNaN(notANumber);

        Console.WriteLine(nanEqualsItself);
        Console.WriteLine(nanDetected);

        // decimal has no infinity and no NaN: the same division throws instead.
        try
        {
            decimal impossible = price / 0m;
            Console.WriteLine(impossible);
        }
        catch (DivideByZeroException exception)
        {
            Console.WriteLine(exception.Message);
        }

        // Rounding: MidpointRounding decides what happens at exactly .5. The default is
        // "to even" (banker's rounding), not the "away from zero" many people expect.
        double midpoint = 2.5;
        double roundedToEven = Math.Round(midpoint);
        double roundedAwayFromZero = Math.Round(midpoint, MidpointRounding.AwayFromZero);
        decimal roundedPrice = Math.Round(price, 1);

        Console.WriteLine(roundedToEven);
        Console.WriteLine(roundedAwayFromZero);
        Console.WriteLine(roundedPrice);

        // Half (since .NET 5) goes the other way: only 16 bit, for graphics and machine learning
        // where memory matters more than precision.
        Half compactValue = (Half)temperature;

        Console.WriteLine(compactValue);
    }
}
