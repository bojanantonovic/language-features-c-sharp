namespace LanguageFeaturesCSharp.Classes;

internal static class PolymorphismWithDelegates
{
    public static void Show()
    {
        // Delegate that points to the (polymorphic) MakeSound method
        Func<Animal, string> soundOf = animal => animal.MakeSound();

        Animal dog = new Dog("Rex");
        Animal cat = new Cat("Minka");

        // The same delegate calls a different implementation depending on the actual type -
        // the polymorphism from Inheritance.cs/Polymorphism.cs works the same way through a delegate.
        string dogSound = soundOf(dog);
        string catSound = soundOf(cat);

        Console.WriteLine(dogSound);
        Console.WriteLine(catSound);

        // But delegates can also carry different behavior directly,
        // with no inheritance at all - a second, independent form of "swappable behavior".
        Func<Animal, string> loudVersion = animal => animal.MakeSound().ToUpper() + "!!!";
        string loudDog = loudVersion(dog);

        Console.WriteLine(loudDog);
    }
}
