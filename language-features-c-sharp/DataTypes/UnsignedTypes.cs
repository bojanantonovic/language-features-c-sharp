using System;

namespace LanguageFeaturesCSharp.DataTypes;

internal static class UnsignedTypes
{
    public static void Show()
    {
        // Unsigned types have no Java equivalent at all: Java only has signed types and works
        // around it with Integer.toUnsignedString / parseUnsignedInt, or by using a wider type.
        // byte = Byte, ushort = UInt16, uint = UInt32, ulong = UInt64.
        byte flags = 255;
        ushort port = 65_535;
        uint fileSize = 4_294_967_295;
        ulong recordCount = 18_446_744_073_709_551_615;

        Console.WriteLine(flags);
        Console.WriteLine(port);
        Console.WriteLine(fileSize);
        Console.WriteLine(recordCount);

        // Dropping the sign buys exactly one more bit of range, not a different size:
        // uint is as wide as int, but starts at 0 instead of at the negative half.
        int largestInt = int.MaxValue;
        uint largestUInt = uint.MaxValue;

        Console.WriteLine(largestInt);
        Console.WriteLine(largestUInt);

        // The U suffix marks a literal as unsigned, UL as unsigned long.
        uint unsignedLiteral = 3_000_000_000U;
        ulong unsignedLongLiteral = 10_000_000_000_000_000_000UL;

        Console.WriteLine(unsignedLiteral);
        Console.WriteLine(unsignedLongLiteral);

        // The typical trap: subtracting below zero does not give a negative number,
        // it wraps around to the largest value - and by default without any error.
        uint zero = 0;
        uint wrappedBelowZero = unchecked(zero - 1);

        Console.WriteLine(wrappedBelowZero);

        // checked makes that trap visible, just like for the signed types.
        try
        {
            uint underflowing = checked(zero - 1);
            Console.WriteLine(underflowing);
        }
        catch (OverflowException exception)
        {
            Console.WriteLine(exception.Message);
        }

        // Converting from a signed type needs an explicit cast, because the value ranges
        // only partly overlap: a negative int turns into a very large uint.
        int negativeNumber = -1;
        uint reinterpreted = unchecked((uint)negativeNumber);

        Console.WriteLine(reinterpreted);

        // Mixing signed and unsigned in one expression: the compiler widens both to long,
        // so the result stays correct instead of silently wrapping.
        long mixedResult = largestUInt + negativeNumber;

        Console.WriteLine(mixedResult);

        // Shifting right is where the missing sign actually helps: on an unsigned type
        // the empty bits are always filled with zeros. Java needs its own operator (>>>) for this,
        // because its >> keeps the sign bit.
        uint unsignedBits = 0xF000_0000;
        uint shiftedUnsigned = unsignedBits >> 4;

        int signedBits = unchecked((int)0xF000_0000);
        int shiftedSigned = signedBits >> 4;

        Console.WriteLine(shiftedUnsigned);
        Console.WriteLine(shiftedSigned);

        // Typical uses: bit masks, colour channels, hashes, file sizes and interop with
        // native APIs - everywhere a negative value would make no sense.
        byte redChannel = 0x8A;
        byte greenChannel = 0x1F;
        byte blueChannel = 0xC3;
        uint colourValue = ((uint)redChannel << 16) | ((uint)greenChannel << 8) | blueChannel;

        Console.WriteLine(colourValue.ToString("X6"));
    }
}
