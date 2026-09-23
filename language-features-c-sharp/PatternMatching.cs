using LanguageFeaturesCSharp.Classes;

namespace LanguageFeaturesCSharp;

internal static class PatternMatching
{
    public static void Show()
    {
        int grade = 2;

        // switch expression: a more compact alternative to the switch statement, returns a value directly.
        // "_" is the catch-all case (equivalent to default).
        string rating = grade switch
        {
            1 => "Very good",
            2 => "Good",
            3 => "Satisfactory",
            _ => "Unknown"
        };

        Console.WriteLine(rating);

        // Type pattern: checks the type and assigns it to a variable at the same time (here: Dog dog)
        Animal animal = new Dog("Rex");
        string description = animal switch
        {
            Dog dog => $"Dog named {dog.Name}",
            Cat cat => $"Cat named {cat.Name}",
            _ => "Unknown animal"
        };

        Console.WriteLine(description);

        // Property pattern: checks properties of an object directly in the pattern,
        // here combined with a relational pattern (< 30)
        Point point = new Point(1, 2);
        string location = point switch
        {
            { X: 0, Y: 0 } => "Origin",
            { X: > 0, Y: > 0 } => "First quadrant",
            _ => "Outside the first quadrant"
        };

        Console.WriteLine(location);

        // is pattern: combines a type check and a condition in one expression
        object value = 42;
        if (value is int number and > 10)
        {
            Console.WriteLine(number);
        }

        // List of mixed animals: LINQ with a pattern in the lambda counts only the dogs.
        // Collection expression ([...]): modern, more compact alternative to "new List<Animal> { ... }".
        List<Animal> animals = [new Dog("Rex"), new Cat("Minka"), new Dog("Bello")];
        int dogCount = animals.Count(a => a is Dog);

        Console.WriteLine(dogCount);
    }
}
