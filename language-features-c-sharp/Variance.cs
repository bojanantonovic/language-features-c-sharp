namespace LanguageFeaturesCSharp;

internal static class Variance
{
    public static void Show()
    {
        // Animal, Dog and Cat are the classes from Inheritance.cs
        List<Dog> dogs = [new Dog("Rex"), new Dog("Bello")];
        List<Cat> cats = [new Cat("Mimi")];

        // Java would write "List<? extends Animal>" at the call site here.
        // C# puts the variance on the type itself: IEnumerable<out T> is declared covariant,
        // so an IEnumerable<Dog> IS an IEnumerable<Animal> - no wildcard needed.
        IEnumerable<Animal> animals = dogs;
        string dogNames = JoinNames(dogs);
        string catNames = JoinNames(cats);

        Console.WriteLine(dogNames);
        Console.WriteLine(catNames);
        Console.WriteLine(animals.Count());

        // "out" means T is only ever returned, never accepted - that is why it is safe
        // to read Dogs as Animals. List<T> itself is not covariant, because Add(T) would let
        // you put a Cat into a List<Dog>; that is exactly what Java's "? extends" prevents
        // by making add(...) uncallable.
        IReadOnlyList<Animal> readOnlyAnimals = dogs;
        Animal firstAnimal = readOnlyAnimals[0];

        Console.WriteLine(firstAnimal.Name);

        // The counterpart of Java's "? super T" is the "in" keyword: IComparer<in T> and
        // Action<in T> only consume their T, so a comparer/action written for Animal
        // can be used wherever one for Dog is expected.
        IComparer<Animal> byName = Comparer<Animal>.Create((left, right) => string.Compare(left.Name, right.Name, StringComparison.Ordinal));
        List<Dog> sortedDogs = [.. dogs];
        sortedDogs.Sort(byName);

        Action<Animal> printSound = animal => Console.WriteLine(animal.MakeSound());
        LetThemSpeak(dogs, printSound);

        Console.WriteLine(sortedDogs[0].Name);

        // Where variance on the type is not enough, a generic method with a constraint
        // gives the same flexibility as a wildcard at the call site.
        string namesViaGenericMethod = JoinNamesOf(dogs);
        Console.WriteLine(namesViaGenericMethod);

        // Arrays are covariant in C# just like in Java - and just as unsafe:
        // the assignment compiles, a wrong write would only fail at runtime.
        Animal[] animalArray = new Dog[2];
        Console.WriteLine(animalArray.Length);
    }

    // Covariance in action: accepts IEnumerable<Dog>, IEnumerable<Cat> and IEnumerable<Animal>
    private static string JoinNames(IEnumerable<Animal> animals)
    {
        return string.Join(", ", animals.Select(animal => animal.Name));
    }

    // Contravariance in action: an Action<Animal> is accepted for the Action<Dog> parameter
    private static void LetThemSpeak(IEnumerable<Dog> dogs, Action<Dog> speak)
    {
        foreach (Dog dog in dogs)
        {
            speak(dog);
        }
    }

    // Generic method with a constraint: the "? extends Animal" of a method signature
    private static string JoinNamesOf<T>(IEnumerable<T> animals) where T : Animal
    {
        return string.Join(", ", animals.Select(animal => animal.Name));
    }
}
