namespace LanguageFeaturesCSharp;

internal static class Methods
{
    public static void Show()
    {
        int square = Square(6);
        int sumOfTwoNumbers = Add(3, 4);
        int sumUpTo10 = SumFrom1To(10);

        Console.WriteLine(square);
        Console.WriteLine(sumOfTwoNumbers);
        Console.WriteLine(sumUpTo10);
    }

    // Method with one parameter and a return value
    private static int Square(int number)
    {
        return number * number;
    }

    // Method with two parameters and a return value
    private static int Add(int a, int b)
    {
        return a + b;
    }

    // Method with a parameter and a return value that uses a loop internally
    // (makes the for loop from Loops.cs reusable for any upper bound)
    private static int SumFrom1To(int limit)
    {
        int sum = 0;
        for (int i = 1; i <= limit; i++)
        {
            sum = sum + i;
        }

        return sum;
    }
}
