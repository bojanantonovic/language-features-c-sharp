namespace LanguageFeaturesCSharp;

// Custom type with properties: Name and Age can be read and written.
internal class Person
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }
}

internal static class Classes
{
    public static void Show()
    {
        // Create an object and set properties individually
        Person alice = new Person();
        alice.Name = "Alice";
        alice.Age = 30;

        // Read properties, result lands in its own variable
        string aliceName = alice.Name;
        int aliceAge = alice.Age;

        // Object initializer: set properties directly at creation
        Person bob = new Person { Name = "Bob", Age = 25 };

        Console.WriteLine(aliceName);
        Console.WriteLine(aliceAge);
        Console.WriteLine(bob.Name);
        Console.WriteLine(bob.Age);

        // List of objects: LINQ works on custom classes too, not just primitive types
        List<Person> people = new List<Person>
        {
            alice,
            bob,
            new Person { Name = "Carol", Age = 40 },
        };

        // OrderByDescending + First: find the person with the highest age
        Person oldest = people.OrderByDescending(person => person.Age).First();

        // Average computed from a number derived from the objects
        double averageAge = people.Average(person => person.Age);

        Console.WriteLine(oldest.Name);
        Console.WriteLine(averageAge);
    }
}
