using System;
using System.Linq;
using LanguageFeaturesCSharp.Classes;

namespace LanguageFeaturesCSharp;

internal static class Parameter
{
    public static void Show()
    {
        // out: the method MUST assign a value to this parameter, meant for extra return values
        bool success = TryDivide(10, 2, out int result);

        Console.WriteLine(success);
        Console.WriteLine(result);

        // ref: the method can read AND change the variable's existing value
        int number = 5;
        Double(ref number);

        Console.WriteLine(number);

        // in: the method only gets the value for reading, avoiding a copy
        Vector2Int a = new Vector2Int(3, 4);
        double length = Length(in a);

        Console.WriteLine(length);

        // Optional parameter: discount has a default value, so it doesn't have to be given
        double priceWithoutDiscount = CalculatePrice(100);
        double priceWithDiscount = CalculatePrice(100, 0.1);

        Console.WriteLine(priceWithoutDiscount);
        Console.WriteLine(priceWithDiscount);

        // params: the method accepts any number of arguments as an array
        int sum = SumAll(1, 2, 3, 4, 5);

        Console.WriteLine(sum);
    }

    private static bool TryDivide(int numerator, int denominator, out int result)
    {
        if (denominator == 0)
        {
            result = 0;
            return false;
        }

        result = numerator / denominator;
        return true;
    }

    private static void Double(ref int value)
    {
        value *= 2;
    }

    private static double Length(in Vector2Int vector)
    {
        return Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y));
    }

    private static double CalculatePrice(double price, double discount = 0.0)
    {
        return price - (price * discount);
    }

    private static int SumAll(params int[] numbers)
    {
        return numbers.Sum();
    }
}
