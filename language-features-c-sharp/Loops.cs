namespace LanguageFeaturesCSharp;

internal static class Loops
{
    public static void Show()
    {
        // for loop: sum of the numbers 1 to 5
        int sum = 0;
        for (int i = 1; i <= 5; i++)
        {
            sum = sum + i;
        }

        // while loop: double until a threshold is reached
        int value = 1;
        while (value < 100)
        {
            value = value * 2;
        }

        // foreach loop: iterate over a list of numbers
        int[] numbers = { 3, 7, 2, 9, 4 };
        int largestNumber = numbers[0];
        foreach (int number in numbers)
        {
            if (number > largestNumber)
            {
                largestNumber = number;
            }
        }

        Console.WriteLine(sum);
        Console.WriteLine(value);
        Console.WriteLine(largestNumber);
    }
}
