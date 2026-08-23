namespace LanguageFeaturesCSharp;

// Custom exception class: inherits from Exception, for a specific error case in our own code
internal class InvalidAgeException : Exception
{
    public InvalidAgeException(string message) : base(message)
    {
    }
}

internal static class Exceptions
{
    public static void Show()
    {
        int resultOk = SafeDivide(10, 2);
        int resultError = SafeDivide(10, 0);

        Console.WriteLine(resultOk);
        Console.WriteLine(resultError);

        try
        {
            ValidateAge(-5);
        }
        catch (InvalidAgeException ex)
        {
            // catches only our own exception, not any exception whatsoever
            Console.WriteLine(ex.Message);
        }
    }

    // throws our own exception if the given value is semantically invalid
    private static void ValidateAge(int age)
    {
        if (age < 0)
        {
            throw new InvalidAgeException($"Age must not be negative: {age}");
        }
    }

    private static int SafeDivide(int numerator, int denominator)
    {
        try
        {
            int result = numerator / denominator;
            return result;
        }
        catch (DivideByZeroException)
        {
            // catch handles the exception instead of letting the program crash
            Console.WriteLine("Error: division by 0 is not allowed.");
            return 0;
        }
        finally
        {
            // finally always runs, whether an exception occurred or not
            Console.WriteLine("SafeDivide was called.");
        }
    }
}
