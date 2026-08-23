namespace LanguageFeaturesCSharp;

internal static class Async
{
    public static async Task ShowAsync()
    {
        int result = await CalculateAsync(6, 7);
        Console.WriteLine(result);
    }

    // async method: can use "await" to wait for an asynchronous operation
    // without blocking the thread. Task<T> is the "container" for the later result.
    private static async Task<int> CalculateAsync(int a, int b)
    {
        await Task.Delay(100); // simulates a long-running operation, e.g. a network call
        return a * b;
    }
}
