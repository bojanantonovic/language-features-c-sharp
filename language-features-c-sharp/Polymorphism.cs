namespace LanguageFeaturesCSharp;

internal static class Polymorphism
{
    public static void Show()
    {
        // List of the base type Animal, but containing objects of different derived types
        List<Animal> animals = new List<Animal>();
        animals.Add(new Dog("Rex"));
        animals.Add(new Cat("Minka"));
        animals.Add(new Dog("Bello"));

        // Polymorphism: the same call to MakeSound() gives a different result depending on the
        // actual type - the compiler only knows Animal here, but at runtime the matching
        // override method of Dog or Cat is executed.
        foreach (Animal animal in animals)
        {
            string sound = animal.MakeSound();
            Console.WriteLine(animal.Name);
            Console.WriteLine(sound);
        }
    }
}
