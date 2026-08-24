namespace LanguageFeaturesCSharp;

// Custom delegate type: only describes the signature of a method (parameters + return value)
internal delegate int CalculationOperation(int a, int b);

// Class with an event: notifies other parts of the code when something changes
internal class Account
{
    public decimal Balance { get; private set; }

    // Event based on a delegate (here: Action<decimal>)
    public event Action<decimal>? BalanceChanged;

    public void Deposit(decimal amount)
    {
        Balance += amount;
        BalanceChanged?.Invoke(Balance); // raises the event, if anyone is listening
    }
}

internal static class Delegates
{
    public static void Show()
    {
        // Delegate: assign to a method and call it like a variable
        CalculationOperation add = Add;
        int sum = add(3, 4);

        // Delegate: assign to a lambda expression
        CalculationOperation multiply = (a, b) => a * b;
        int product = multiply(3, 4);

        Console.WriteLine(sum);
        Console.WriteLine(product);

        // List of delegates: LINQ can apply each one to the same arguments.
        // Collection expression ([...]): modern, more compact alternative to "new List<...> { ... }".
        List<CalculationOperation> operations =
        [
            Add,
            (a, b) => a * b,
            (a, b) => a - b,
        ];

        List<int> results = operations.Select(operation => operation(10, 3)).ToList();

        Console.WriteLine(string.Join(", ", results));

        // Subscribe to the event: the lambda is called as soon as BalanceChanged is raised
        Account account = new Account();
        account.BalanceChanged += newBalance => Console.WriteLine(newBalance);

        account.Deposit(100);
        account.Deposit(50);
    }

    private static int Add(int a, int b)
    {
        return a + b;
    }
}
