using System;

namespace LanguageFeaturesCSharp.DataTypes;

internal static class ObjectType
{
    public static void Show()
    {
        // object is the alias for System.Object and the root of the whole type system.
        // Unlike Java, this includes the number types: int really derives from object,
        // there is no split into "primitive int" and "wrapper Integer".
        int number = 42;
        Type numberType = number.GetType();

        Console.WriteLine(numberType.Name);
        Console.WriteLine(numberType.FullName);

        // The alias and the framework name are the same type, not two related ones.
        bool aliasIsSameType = typeof(int) == typeof(Int32);

        Console.WriteLine(aliasIsSameType);

        // Boxing: the value is copied into an object on the heap so it can be stored
        // in an object variable. Java does the same, but wraps the int into an Integer.
        object boxedNumber = number;

        // Unboxing: the cast back copies the value out of the box again.
        int unboxedNumber = (int)boxedNumber;

        Console.WriteLine(boxedNumber);
        Console.WriteLine(unboxedNumber);

        // The box is a real object: it knows its type and answers the object members.
        string boxedTypeName = boxedNumber.GetType().Name;

        Console.WriteLine(boxedTypeName);

        // Every type inherits four methods from object: ToString, Equals, GetHashCode, GetType.
        // For int they are overridden in a meaningful way, so the text is the number itself.
        string numberAsText = number.ToString();
        int numberHashCode = number.GetHashCode();

        Console.WriteLine(numberAsText);
        Console.WriteLine(numberHashCode);

        // Equals compares values for value types and for string, but references for a plain class
        // (Java: equals vs ==, with the same trap for classes that do not override equals).
        string leftText = "Hello";
        string rightText = string.Concat("Hel", "lo");
        bool textsAreEqual = leftText.Equals(rightText);

        Console.WriteLine(textsAreEqual);

        // ReferenceEquals asks the other question: is it the very same instance?
        // The two boxes below hold the same value, but are two separate objects.
        object firstBox = number;
        object secondBox = number;
        bool sameInstance = ReferenceEquals(firstBox, secondBox);
        bool equalValues = firstBox.Equals(secondBox);

        Console.WriteLine(sameInstance);
        Console.WriteLine(equalValues);

        // An object variable can hold anything; the type pattern gets the value back safely
        // (see PatternMatching.cs), which is C#'s counterpart to "instanceof" plus a cast.
        object[] mixedValues = [7, "Text", 1.5, true];
        int numberCount = 0;

        foreach (object value in mixedValues)
        {
            if (value is int)
            {
                numberCount++;
            }
        }

        Console.WriteLine(numberCount);
        Console.WriteLine(string.Join(", ", mixedValues));
    }
}
