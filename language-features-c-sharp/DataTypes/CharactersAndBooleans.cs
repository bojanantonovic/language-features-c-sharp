using System;
using System.Text;

namespace LanguageFeaturesCSharp.DataTypes;

internal static class CharactersAndBooleans
{
    public static void Show()
    {
        // char = Char: 16 bit, one UTF-16 code unit - the same model as in Java.
        // Single quotes mean char, double quotes mean string.
        char firstLetter = 'A';
        char digitCharacter = '7';

        Console.WriteLine(firstLetter);
        Console.WriteLine(digitCharacter);

        // Internally a char is a number, so it can be converted and compared like one.
        int letterCode = firstLetter;
        char nextLetter = (char)(firstLetter + 1);

        Console.WriteLine(letterCode);
        Console.WriteLine(nextLetter);

        // The static helpers replace Java's Character.isDigit(...) and friends.
        bool isDigit = char.IsDigit(digitCharacter);
        bool isLetter = char.IsLetter(firstLetter);
        char lowerCaseLetter = char.ToLowerInvariant(firstLetter);

        Console.WriteLine(isDigit);
        Console.WriteLine(isLetter);
        Console.WriteLine(lowerCaseLetter);

        // Escape sequences and the unicode escape work as usual.
        char tabulator = '\t';
        char omega = 'Ω';

        Console.WriteLine(omega);
        Console.WriteLine($"Column1{tabulator}Column2");

        // A string is a sequence of chars, so indexing gives a char, not a one-letter string.
        string word = "Beispiel";
        char thirdCharacter = word[2];
        int characterCount = word.Length;

        Console.WriteLine(thirdCharacter);
        Console.WriteLine(characterCount);

        // The limit of 16 bit: characters outside the basic plane need two chars
        // (a surrogate pair), so Length counts code units, not visible characters.
        string emoji = "🙂";
        int emojiLength = emoji.Length;

        Console.WriteLine(emojiLength);

        // Rune (since .NET 3.0) stands for a complete unicode code point and is the way
        // out of that trap - Java solves the same problem with int code points.
        int runeCount = 0;

        foreach (Rune rune in emoji.EnumerateRunes())
        {
            runeCount++;
            Console.WriteLine(rune.Value);
        }

        Console.WriteLine(runeCount);

        // bool = Boolean: only true or false. Unlike C, a number is not a condition -
        // "if (1)" does not compile, exactly as in Java.
        bool isActive = true;
        bool isFinished = false;

        Console.WriteLine(isActive);
        Console.WriteLine(isFinished);

        // && and || evaluate lazily: the right-hand side is skipped once the result is settled,
        // & and | always evaluate both sides.
        bool lazyResult = isFinished && HasSideEffect();
        bool eagerResult = isFinished & HasSideEffect();

        Console.WriteLine(lazyResult);
        Console.WriteLine(eagerResult);

        // Parsing and formatting go through the type itself, not through a wrapper class.
        bool parsedFlag = bool.Parse("true");
        string flagAsText = isActive.ToString();

        Console.WriteLine(parsedFlag);
        Console.WriteLine(flagAsText);
    }

    // Marks whether it ran, to make the difference between && and & visible
    private static bool HasSideEffect()
    {
        Console.WriteLine("side effect");

        return true;
    }
}
