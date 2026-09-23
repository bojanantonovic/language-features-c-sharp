namespace LanguageFeaturesCSharp.Classes;

// Interface: only specifies WHAT a class must be able to do, not HOW (no shared base class needed)
internal interface IVehicle
{
    string Move();
}

internal class Car : IVehicle
{
    public string Move()
    {
        return "drives on the road";
    }
}

internal class Bicycle : IVehicle
{
    public string Move()
    {
        return "rides on the bike path";
    }
}

internal static class Interfaces
{
    public static void Show()
    {
        // List of the interface type: Car and Bicycle have no shared base class,
        // but both fulfil the IVehicle contract.
        // Collection expression ([...]): modern, more compact alternative to "new List<T>()" + repeated Add(...).
        List<IVehicle> vehicles = [new Car(), new Bicycle()];

        foreach (IVehicle vehicle in vehicles)
        {
            string movement = vehicle.Move();
            Console.WriteLine(movement);
        }

        // Select: turns each vehicle (via the interface) into its movement text
        List<string> movements = vehicles.Select(vehicle => vehicle.Move()).ToList();

        // OfType<T>: filters just the cars out of the interface list
        int carCount = vehicles.OfType<Car>().Count();

        Console.WriteLine(string.Join(" / ", movements));
        Console.WriteLine(carCount);
    }
}
