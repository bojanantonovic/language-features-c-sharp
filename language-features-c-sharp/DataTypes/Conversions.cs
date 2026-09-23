using System;
using System.Globalization;

namespace LanguageFeaturesCSharp.DataTypes;

internal static class Conversions
{
    public static void Show()
    {
        // Widening happens implicitly, because no information can be lost:
        // int fits into long, long fits into double.
        int number = 1_234;
        long widenedToLong = number;
        double widenedToDouble = widenedToLong;

        Console.WriteLine(widenedToLong);
        Console.WriteLine(widenedToDouble);

        // Narrowing needs an explicit cast and cuts off the fraction - it does not round.
        double measurement = 9.87;
        int truncated = (int)measurement;
        int rounded = (int)Math.Round(measurement);

        Console.WriteLine(truncated);
        Console.WriteLine(rounded);

        // A cast that does not fit wraps silently by default; checked makes it an exception.
        int tooLargeForByte = 300;
        byte wrappedByte = unchecked((byte)tooLargeForByte);

        Console.WriteLine(wrappedByte);

        try
        {
            byte checkedByte = checked((byte)tooLargeForByte);
            Console.WriteLine(checkedByte);
        }
        catch (OverflowException exception)
        {
            Console.WriteLine(exception.Message);
        }

        // decimal and double never convert into each other implicitly, in either direction:
        // one would lose precision, the other range.
        decimal price = 19.99m;
        double priceAsDouble = (double)price;

        Console.WriteLine(priceAsDouble);

        // Text to number: Parse throws on bad input, the equivalent of Integer.parseInt.
        int parsedNumber = int.Parse("42");

        Console.WriteLine(parsedNumber);

        try
        {
            int invalidNumber = int.Parse("abc");
            Console.WriteLine(invalidNumber);
        }
        catch (FormatException exception)
        {
            Console.WriteLine(exception.Message);
        }

        // TryParse reports failure through its return value instead of an exception,
        // and writes the result into an out parameter - Java has no direct counterpart.
        bool couldParse = int.TryParse("abc", out int failedResult);

        Console.WriteLine(couldParse);
        Console.WriteLine(failedResult);

        // Convert is the third way: it also accepts null and turns it into 0 instead of failing.
        string? missingValue = null;
        int convertedFromNull = Convert.ToInt32(missingValue);

        Console.WriteLine(convertedFromNull);

        // Parsing and formatting depend on the culture, just like Java's Locale.
        // The invariant culture keeps data exchange independent of the machine's settings.
        // Language alone is not enough: de-DE separates decimals with a comma,
        // de-CH with a point - so the same text parses in one culture and fails in the other.
        double germanStyle = double.Parse("3,14", CultureInfo.GetCultureInfo("de-DE"));
        double swissStyle = double.Parse("3.14", CultureInfo.GetCultureInfo("de-CH"));
        double invariantStyle = double.Parse("3.14", CultureInfo.InvariantCulture);

        Console.WriteLine(germanStyle == invariantStyle);
        Console.WriteLine(swissStyle == invariantStyle);

        // The same applies the other way round when producing text.
        string invariantText = invariantStyle.ToString(CultureInfo.InvariantCulture);
        string germanText = invariantStyle.ToString(CultureInfo.GetCultureInfo("de-DE"));
        string swissCurrencyText = price.ToString("C", CultureInfo.GetCultureInfo("de-CH"));

        Console.WriteLine(invariantText);
        Console.WriteLine(germanText);
        Console.WriteLine(swissCurrencyText);

        // Format strings control the shape of the number: N2 groups and keeps 2 decimals,
        // X prints hexadecimal, P a percentage.
        string groupedText = 1_234_567.891.ToString("N2", CultureInfo.InvariantCulture);
        string hexText = 255.ToString("X");
        string percentageText = 0.075.ToString("P1", CultureInfo.InvariantCulture);

        Console.WriteLine(groupedText);
        Console.WriteLine(hexText);
        Console.WriteLine(percentageText);
    }
}
