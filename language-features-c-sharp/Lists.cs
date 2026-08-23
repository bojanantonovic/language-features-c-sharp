namespace LanguageFeaturesCSharp;

internal static class Lists
{
    public static void Show()
    {
        // Array: fixed size, filled directly at declaration
        string[] colors = { "Red", "Green", "Blue" };
        string firstColor = colors[0];
        int colorCount = colors.Length;

        // List<T>: variable size, elements can be added/removed
        List<string> shoppingList = new List<string>();
        shoppingList.Add("Milk");
        shoppingList.Add("Bread");
        shoppingList.Add("Butter");
        shoppingList.Remove("Bread");
        int shoppingListCount = shoppingList.Count;

        Console.WriteLine(firstColor);
        Console.WriteLine(colorCount);
        Console.WriteLine(shoppingListCount);

        foreach (string item in shoppingList)
        {
            Console.WriteLine(item);
        }
    }
}
