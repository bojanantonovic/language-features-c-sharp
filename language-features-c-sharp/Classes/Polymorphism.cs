namespace LanguageFeaturesCSharp.Classes;

internal static class Polymorphism
{
    public static void Show()
    {
        // List of the base type Animal, but containing objects of different derived types.
        // Collection expression ([...]): modern, more compact alternative to "new List<T>()" + repeated Add(...).
        List<Animal> animals = [new Dog("Rex"), new Cat("Minka"), new Dog("Bello")];

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
