namespace LanguageFeaturesCSharp;

// Extension methods must be in a static class. The first parameter with
// "this" in front determines which type the method extends (here: string).
internal static class StringExtensions
{
    public static string Reverse(this string text)
    {
        char[] characters = text.ToCharArray();
        Array.Reverse(characters);
        return new string(characters);
    }

    public static bool IsPalindrome(this string text)
    {
        string reversed = text.Reverse();
        return string.Equals(text, reversed, StringComparison.OrdinalIgnoreCase);
    }
}

internal static class Extensions
{
    public static void Show()
    {
        string word = "Anna";

        // looks like a completely normal method call on string,
        // even though string itself was not changed
        bool isPalindrome = word.IsPalindrome();
        string reversed = word.Reverse();

        Console.WriteLine(isPalindrome);
        Console.WriteLine(reversed);

        // List of words: the extension methods can be used directly in LINQ lambdas
        List<string> words = new List<string> { "Anna", "Otto", "Haus", "Level", "Baum" };

        List<string> palindromes = words.Where(w => w.IsPalindrome()).ToList();
        List<string> reversedWords = words.Select(w => w.Reverse()).ToList();

        Console.WriteLine(string.Join(", ", palindromes));
        Console.WriteLine(string.Join(", ", reversedWords));
    }
}
