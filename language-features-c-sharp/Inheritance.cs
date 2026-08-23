namespace LanguageFeaturesCSharp;

// Base class
internal class Animal
{
    public string Name { get; set; }

    public Animal(string name)
    {
        Name = name;
    }

    // virtual: may be overridden by derived classes
    public virtual string MakeSound()
    {
        return "...";
    }
}

// Derived class: inherits from Animal, "base(...)" calls the base class constructor
internal class Dog : Animal
{
    public Dog(string name) : base(name)
    {
    }

    // override: replaces the base implementation of MakeSound
    public override string MakeSound()
    {
        return "Woof";
    }
}

internal class Cat : Animal
{
    public Cat(string name) : base(name)
    {
    }

    public override string MakeSound()
    {
        return "Meow";
    }
}

internal static class Inheritance
{
    public static void Show()
    {
        Dog dog = new Dog("Rex");
        Cat cat = new Cat("Minka");

        string dogSound = dog.MakeSound();
        string catSound = cat.MakeSound();

        Console.WriteLine(dog.Name);
        Console.WriteLine(dogSound);
        Console.WriteLine(cat.Name);
        Console.WriteLine(catSound);

        // List of the derived type Dog: LINQ uses the Name property inherited from Animal directly
        List<Dog> dogs = new List<Dog>
        {
            new Dog("Rex"),
            new Dog("Bello"),
            new Dog("Ari"),
        };

        List<string> sortedDogNames = dogs
            .OrderBy(d => d.Name)
            .Select(d => d.Name)
            .ToList();

        Console.WriteLine(string.Join(", ", sortedDogNames));
    }
}
