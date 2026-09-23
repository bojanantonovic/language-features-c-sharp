using System;
using System.Numerics;

namespace LanguageFeaturesCSharp.DataTypes;

internal static class IntegerTypes
{
    public static void Show()
    {
        // The signed integer types, from small to large. The keyword on the left is only an alias:
        // sbyte = SByte, short = Int16, int = Int32, long = Int64.
        sbyte tinyNumber = -128;
        short smallNumber = -32_768;
        int number = -2_147_483_648;
        long bigNumber = -9_223_372_036_854_775_808;

        Console.WriteLine(tinyNumber);
        Console.WriteLine(smallNumber);
        Console.WriteLine(number);
        Console.WriteLine(bigNumber);

        // Every numeric type carries its own range as a constant, instead of a separate
        // wrapper class like Java's Integer.MIN_VALUE / Integer.MAX_VALUE.
        int smallestInt = int.MinValue;
        int largestInt = int.MaxValue;
        int bytesPerInt = sizeof(int);

        Console.WriteLine(smallestInt);
        Console.WriteLine(largestInt);
        Console.WriteLine(bytesPerInt);

        // Careful: C#'s byte is UNSIGNED (0 to 255). Java's signed byte is sbyte here.
        byte smallestByte = byte.MinValue;
        byte largestByte = byte.MaxValue;

        Console.WriteLine(smallestByte);
        Console.WriteLine(largestByte);

        // Literals: _ groups digits, 0x is hexadecimal, 0b is binary,
        // the L suffix makes a literal long (needed above int.MaxValue).
        int oneMillion = 1_000_000;
        int hexValue = 0xFF;
        int binaryValue = 0b1010_1010;
        long longLiteral = 3_000_000_000L;

        Console.WriteLine(oneMillion);
        Console.WriteLine(hexValue);
        Console.WriteLine(binaryValue);
        Console.WriteLine(longLiteral);

        // Integer division truncates, it does not round - the remainder comes from %.
        int quotient = 7 / 2;
        int remainder = 7 % 2;

        Console.WriteLine(quotient);
        Console.WriteLine(remainder);

        // By default an overflow wraps around silently, exactly like in Java.
        int wrappedValue = unchecked(int.MaxValue + 1);

        Console.WriteLine(wrappedValue);

        // checked turns the same overflow into an exception. Java has no such block;
        // it offers only single methods like Math.addExact.
        // The value comes from a variable here, because the compiler already rejects
        // a constant overflow like "checked(int.MaxValue + 1)" at compile time.
        try
        {
            int overflowing = checked(largestInt + 1);
            Console.WriteLine(overflowing);
        }
        catch (OverflowException exception)
        {
            Console.WriteLine(exception.Message);
        }

        // Int128 (since .NET 7) extends the range once more, still as a fixed-size value type.
        Int128 hugeNumber = Int128.MaxValue;

        Console.WriteLine(hugeNumber);

        // BigInteger grows as needed and is limited only by memory - the counterpart
        // of java.math.BigInteger, but with the normal operators instead of add()/multiply().
        BigInteger factorialOf25 = 1;

        for (int factor = 2; factor <= 25; factor++)
        {
            factorialOf25 *= factor;
        }

        Console.WriteLine(factorialOf25);
    }
}
