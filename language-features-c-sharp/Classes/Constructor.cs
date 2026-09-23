using System;

namespace LanguageFeaturesCSharp.Classes;

// Class with constructors: properties are set directly when the object is created.
internal class Book
{
    public string Title { get; set; }

    public int PageCount { get; set; }

    // Constructor with parameters
    public Book(string title, int pageCount)
    {
        Title = title;
        PageCount = pageCount;
    }

    // Overloaded constructor: calls the other constructor via "this(...)"
    // and sets a default value for the page count while doing so.
    public Book(string title) : this(title, 0)
    {
    }
}

internal static class Constructor
{
    public static void Show()
    {
        Book novel = new Book("Der Steppenwolf", 320);
        Book unknown = new Book("Unknown Book");

        string novelTitle = novel.Title;
        int novelPageCount = novel.PageCount;

        Console.WriteLine(novelTitle);
        Console.WriteLine(novelPageCount);
        Console.WriteLine(unknown.Title);
        Console.WriteLine(unknown.PageCount);
    }
}
