namespace LanguageFeaturesCSharp;

// record: compact syntax for an immutable data type with automatic value equality
internal record Address(string Street, string City);

internal static class Records
{
    public static void Show()
    {
        Address address1 = new Address("Station Street 1", "Zurich");
        Address address2 = new Address("Station Street 1", "Zurich");

        // records compare their values, not the reference (unlike a regular class)
        bool areEqual = address1 == address2;

        // with expression: creates a new copy with a changed property,
        // the original object stays unchanged
        Address address3 = address1 with { City = "Bern" };

        Console.WriteLine(areEqual);
        Console.WriteLine(address1.City);
        Console.WriteLine(address3.City);
        Console.WriteLine(address1);

        // List of records: LINQ works on them just like on int in Collections.cs
        List<Address> addresses = new List<Address>
        {
            new Address("Station Street 1", "Zurich"),
            new Address("Market Street 5", "Bern"),
            new Address("Lake Street 12", "Zurich"),
        };

        // Where + Select: filter first, then pull just the street out of the record
        List<string> zurichStreets = addresses
            .Where(address => address.City == "Zurich")
            .Select(address => address.Street)
            .ToList();

        // GroupBy: groups the addresses by a property of the record
        var addressesByCity = addresses.GroupBy(address => address.City);

        Console.WriteLine(string.Join(", ", zurichStreets));

        foreach (var group in addressesByCity)
        {
            Console.WriteLine($"{group.Key}: {group.Count()}");
        }
    }
}
