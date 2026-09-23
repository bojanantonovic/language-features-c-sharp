using System;
using System.Collections.Generic;

namespace LanguageFeaturesCSharp.DataTypes;

internal static class BuiltInStructs
{
    public static void Show()
    {
        // Every keyword is only an alias for a struct in the System namespace.
        // Int32 and int are not two related types, they are the same type - so the two
        // variables can be assigned to each other without any conversion.
        Int32 number = 42;
        int sameNumber = number;

        Console.WriteLine(number);
        Console.WriteLine(sameNumber);
        Console.WriteLine(number == sameNumber);

        // The whole family, written with the struct name instead of the keyword.
        SByte tinyNumber = -128;
        Byte smallUnsignedNumber = 255;
        Int16 shortNumber = -32_768;
        UInt16 portNumber = 65_535;
        UInt32 fileSize = 4_294_967_295;
        Int64 bigNumber = -9_223_372_036_854_775_808;
        UInt64 recordCount = 18_446_744_073_709_551_615;
        Single temperature = 21.5f;
        Double distance = 384_400.0;
        Decimal price = 19.99m;
        Boolean isActive = true;
        Char firstLetter = 'A';

        Console.WriteLine(tinyNumber);
        Console.WriteLine(smallUnsignedNumber);
        Console.WriteLine(shortNumber);
        Console.WriteLine(portNumber);
        Console.WriteLine(fileSize);
        Console.WriteLine(bigNumber);
        Console.WriteLine(recordCount);
        Console.WriteLine(temperature);
        Console.WriteLine(distance);
        Console.WriteLine(price);
        Console.WriteLine(isActive);
        Console.WriteLine(firstLetter);

        // At runtime only the struct name exists: the keyword never appears again.
        string typeNameOfKeyword = sameNumber.GetType().Name;
        string typeNameOfStruct = number.GetType().Name;
        bool sameTypeObject = typeof(uint) == typeof(UInt32);

        Console.WriteLine(typeNameOfKeyword);
        Console.WriteLine(typeNameOfStruct);
        Console.WriteLine(sameTypeObject);

        // The static members belong to the struct, not to a separate wrapper class
        // like Java's Integer - UInt32.MaxValue instead of Integer.MAX_VALUE.
        UInt32 largestFileSize = UInt32.MaxValue;
        UInt32 smallestFileSize = UInt32.MinValue;
        UInt32 parsedFileSize = UInt32.Parse("3000000000");
        bool couldParse = UInt32.TryParse("not a number", out UInt32 failedFileSize);

        Console.WriteLine(largestFileSize);
        Console.WriteLine(smallestFileSize);
        Console.WriteLine(parsedFileSize);
        Console.WriteLine(couldParse);
        Console.WriteLine(failedFileSize);

        // And the instance members come from the struct as well, callable on any value.
        string fileSizeAsText = fileSize.ToString();
        string fileSizeAsHex = fileSize.ToString("X8");
        int comparison = fileSize.CompareTo(1_000u);
        bool valuesAreEqual = fileSize.Equals(4_294_967_295u);

        Console.WriteLine(fileSizeAsText);
        Console.WriteLine(fileSizeAsHex);
        Console.WriteLine(comparison);
        Console.WriteLine(valuesAreEqual);

        // UInt32 is a struct, so it behaves like one: it derives from ValueType,
        // it can never be null, and it has a zero value it falls back to.
        bool isValueType = typeof(UInt32).IsValueType;
        string? baseTypeName = typeof(UInt32).BaseType?.FullName;
        UInt32 defaultFileSize = default;

        Console.WriteLine(isValueType);
        Console.WriteLine(baseTypeName);
        Console.WriteLine(defaultFileSize);

        // Assigning copies the value; the copy is independent of the original.
        UInt32 originalSize = 10;
        UInt32 copiedSize = originalSize;
        copiedSize += 5;

        Console.WriteLine(originalSize);
        Console.WriteLine(copiedSize);

        // Nullable<UInt32> - written UInt32? - adds the null that the struct itself does not have.
        UInt32? missingFileSize = null;
        UInt32 fileSizeOrDefault = missingFileSize ?? 0;

        Console.WriteLine(missingFileSize.HasValue);
        Console.WriteLine(fileSizeOrDefault);

        // The struct name is what generic types and methods use, exactly like any other type.
        List<UInt32> fileSizes = [1_024u, 2_048u, 4_096u];
        Dictionary<String, UInt32> sizeByFileName = new Dictionary<String, UInt32>
        {
            ["report.pdf"] = 240_128u,
            ["photo.jpg"] = 3_145_728u
        };

        Console.WriteLine(string.Join(", ", fileSizes));
        Console.WriteLine(sizeByFileName["photo.jpg"]);

        // Where the framework needs the type as a NAME, only the struct spelling works -
        // there is no method called "ToUint" or a "uint" suffix anywhere.
        UInt32 convertedFileSize = Convert.ToUInt32("255");
        Byte[] fileSizeBytes = BitConverter.GetBytes(fileSize);
        UInt32 fileSizeFromBytes = BitConverter.ToUInt32(fileSizeBytes);

        Console.WriteLine(convertedFileSize);
        Console.WriteLine(fileSizeBytes.Length);
        Console.WriteLine(fileSizeFromBytes);

        // Object and String are the two aliases that are NOT structs: object is the base class
        // of everything (see ObjectType.cs) and string is a class, so both can be null.
        bool objectIsValueType = typeof(Object).IsValueType;
        bool stringIsValueType = typeof(String).IsValueType;

        Console.WriteLine(objectIsValueType);
        Console.WriteLine(stringIsValueType);

        // Which spelling to use: C# style is the keyword (uint, int, string) for declarations,
        // and the struct name where it reads as a name - UInt32.MaxValue, Convert.ToUInt32(...).
    }
}
