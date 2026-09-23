using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LanguageFeaturesCSharp;

internal static class Async
{
    private const string WorkingDirectoryName = "language-features-async";
    private const int StreamBufferSize = 4096;

    // One HttpClient for the whole application: it manages the connection pool.
    // A new HttpClient per request exhausts the sockets ("socket exhaustion").
    private static readonly HttpClient SharedHttpClient = CreateHttpClient();

    public static async Task ShowAsync()
    {
        string workingDirectory = CreateWorkingDirectory();

        try
        {
            await PrepareSampleFilesAsync(workingDirectory);

            await FileBasicsAsync(workingDirectory);

            try
            {
                await ParallelRequestsAsync();
                await FastestSourceAsync();
                await CancellationAsync();
                await LimitedParallelismAsync(workingDirectory);
            }
            catch (HttpRequestException exception)
            {
                // One catch for every section that needs the network: without a
                // connection they cannot run, the rest of the demo is unaffected.
                Console.WriteLine($"no network: {exception.Message}");
            }

            await ErrorHandlingAsync(workingDirectory);
            await StreamingAsync(workingDirectory);
            await CpuBoundWorkAsync(workingDirectory);
            await ProgressReportingAsync(workingDirectory);
            await CachedValueTaskAsync(workingDirectory);
        }
        finally
        {
            Directory.Delete(workingDirectory, recursive: true);
        }
    }

    // ---------------------------------------------------------------------
    // 1. Basics: reading and writing files without blocking a thread
    // ---------------------------------------------------------------------

    private const string NotesFileName = "notes.txt";
    private const string ShoppingListFileName = "shopping-list.txt";

    private static async Task FileBasicsAsync(string workingDirectory)
    {
        string notesPath = Path.Combine(workingDirectory, NotesFileName);

        // An async method does NOT start a thread. It runs synchronously until the first
        // await; there it returns to the caller and continues when the operation is done.
        // While the operating system is busy with the disk, the thread is free for other
        // work - that is the whole point of async for I/O.
        await File.WriteAllTextAsync(notesPath, "async is about waiting, not about threads");
        string notes = await File.ReadAllTextAsync(notesPath);

        Console.WriteLine(notes);

        // await using: the StreamWriter still holds a buffer when the block ends -
        // DisposeAsync writes it to disk asynchronously.
        string shoppingListPath = Path.Combine(workingDirectory, ShoppingListFileName);
        string[] items = ["coffee", "milk", "bread"];

        await using StreamWriter writer = new(shoppingListPath);

        foreach (string item in items)
        {
            await writer.WriteLineAsync(item);
        }
    }

    // ---------------------------------------------------------------------
    // 2. Use case: independent requests at the same time (Task.WhenAll)
    // ---------------------------------------------------------------------

    private const string GitHubRepositoryUrlPrefix = "https://api.github.com/repos/";
    private const string UserAgentHeaderName = "User-Agent";
    private const string UserAgentValue = "language-features-c-sharp";
    private const int HttpTimeoutSeconds = 10;

    private static readonly string[] Repositories = ["dotnet/runtime", "dotnet/roslyn", "dotnet/aspnetcore"];

    private static HttpClient CreateHttpClient()
    {
        HttpClient client = new() { Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds) };
        client.DefaultRequestHeaders.Add(UserAgentHeaderName, UserAgentValue); // the GitHub API insists on this

        return client;
    }

    private static async Task ParallelRequestsAsync()
    {
        // Sequential: every await waits for the previous answer, the latencies add up.
        Stopwatch sequentialWatch = Stopwatch.StartNew();

        foreach (string repositoryName in Repositories)
        {
            await LoadRepositoryAsync(repositoryName);
        }

        sequentialWatch.Stop();

        // Parallel: calling the method already starts the request, only WhenAll waits.
        // All three are in flight, so the total time is roughly the slowest one
        // instead of the sum of all three.
        Stopwatch parallelWatch = Stopwatch.StartNew();
        GitHubRepository?[] repositories = await Task.WhenAll(Repositories.Select(name => LoadRepositoryAsync(name)));
        parallelWatch.Stop();

        int starCount = repositories.Sum(repository => repository?.StarCount ?? 0);

        Console.WriteLine($"{repositories.Length} repositories, {starCount} stars: sequential {sequentialWatch.ElapsedMilliseconds} ms, parallel {parallelWatch.ElapsedMilliseconds} ms");
    }

    // GetFromJsonAsync reads the response stream and deserializes it in one step.
    private static Task<GitHubRepository?> LoadRepositoryAsync(string repositoryName, CancellationToken cancellationToken = default)
    {
        return SharedHttpClient.GetFromJsonAsync<GitHubRepository>(GitHubRepositoryUrlPrefix + repositoryName, cancellationToken);
    }

    // ---------------------------------------------------------------------
    // 3. Use case: whichever source answers first wins (Task.WhenAny)
    // ---------------------------------------------------------------------

    private const string ExampleComUrl = "https://example.com";
    private const string ExampleOrgUrl = "https://example.org";

    private static async Task FastestSourceAsync()
    {
        // Both mirrors deliver the same content; whoever is faster decides.
        using CancellationTokenSource cancellation = new();

        Task<string> comSource = SharedHttpClient.GetStringAsync(ExampleComUrl, cancellation.Token);
        Task<string> orgSource = SharedHttpClient.GetStringAsync(ExampleOrgUrl, cancellation.Token);

        // WhenAny returns the task that finished first - not its result.
        Task<string> firstFinished = await Task.WhenAny(comSource, orgSource);

        // The extra await unwraps the result and rethrows a possible exception.
        string content = await firstFinished;
        string winner = firstFinished == comSource ? ExampleComUrl : ExampleOrgUrl;

        Console.WriteLine($"fastest source: {winner} with {content.Length} characters");

        // The loser is still running: cancelling frees the connection...
        await cancellation.CancelAsync();
        Task<string> loser = firstFinished == comSource ? orgSource : comSource;

        try
        {
            // ...and awaiting it makes sure its error does not stay unobserved.
            await loser;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("slower request cancelled");
        }
    }

    // ---------------------------------------------------------------------
    // 4. Use case: timeout and cancellation (CancellationToken)
    // ---------------------------------------------------------------------

    private const int ShortTimeoutMilliseconds = 1;

    private static async Task CancellationAsync()
    {
        // A token source that cancels itself after a given time is how a timeout is
        // built. The token travels through every layer down to the actual request.
        using CancellationTokenSource timeout = new(TimeSpan.FromMilliseconds(ShortTimeoutMilliseconds));

        try
        {
            await LoadRepositoryAsync(Repositories[0], timeout.Token);
            Console.WriteLine("answer arrived within the timeout");
        }
        catch (OperationCanceledException)
        {
            // Cancellation is not a bug: it is the expected end of the operation.
            // HttpClient reports it as TaskCanceledException, a subclass of this one.
            Console.WriteLine($"request cancelled after {ShortTimeoutMilliseconds} ms");
        }

        // The same mechanism, triggered from outside instead of by a clock: a "Cancel"
        // button, a shutdown signal, or - as here - a result that is no longer needed.
        using CancellationTokenSource cancellation = new();
        Task<GitHubRepository?> runningRequest = LoadRepositoryAsync(Repositories[1], cancellation.Token);
        await cancellation.CancelAsync();

        try
        {
            await runningRequest;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"running request cancelled (state: {runningRequest.Status})");
        }
    }

    // ---------------------------------------------------------------------
    // 5. Use case: many calls, but not all at once (limited parallelism)
    // ---------------------------------------------------------------------

    private const int MaxParallelRequests = 2;
    private const int MaxParallelFileReads = 2;

    private static readonly string[] ManyRepositories =
    [
        "dotnet/runtime", "dotnet/roslyn", "dotnet/aspnetcore", "dotnet/efcore", "dotnet/maui", "dotnet/sdk"
    ];

    private static async Task LimitedParallelismAsync(string workingDirectory)
    {
        // Firing all six requests at once would be fast - and would run straight into
        // the rate limit of the API. Parallel.ForEachAsync keeps a fixed maximum
        // number of operations in flight.
        ConcurrentBag<int> starCounts = []; // several tasks write at the same time
        ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = MaxParallelRequests };

        await Parallel.ForEachAsync(ManyRepositories, parallelOptions, async (repositoryName, cancellationToken) =>
        {
            GitHubRepository? repository = await LoadRepositoryAsync(repositoryName, cancellationToken);

            if (repository is not null)
            {
                starCounts.Add(repository.StarCount);
            }
        });

        Console.WriteLine($"{starCounts.Count} repositories, max {MaxParallelRequests} requests at a time");

        // The classic alternative when the tasks do not come from a loop over a
        // collection: a SemaphoreSlim as a ticket counter in front of the actual call.
        using SemaphoreSlim limiter = new(MaxParallelFileReads);

        IEnumerable<Task<string>> readTasks = ReportFileNames.Select(async fileName =>
        {
            await limiter.WaitAsync();

            try
            {
                return await File.ReadAllTextAsync(Path.Combine(workingDirectory, fileName));
            }
            finally
            {
                limiter.Release(); // release the ticket in finally, otherwise it leaks on an error
            }
        });

        string[] contents = await Task.WhenAll(readTasks);

        Console.WriteLine($"{contents.Length} files read, max {MaxParallelFileReads} at a time");
    }

    // ---------------------------------------------------------------------
    // 6. Use case: error handling in asynchronous code
    // ---------------------------------------------------------------------

    private const string MissingFileName = "does-not-exist.txt";
    private const string OtherMissingFileName = "also-missing.txt";

    private static async Task ErrorHandlingAsync(string workingDirectory)
    {
        // try/catch around an await works exactly like in synchronous code:
        // await rethrows the original exception, not a wrapper around it.
        try
        {
            await File.ReadAllTextAsync(Path.Combine(workingDirectory, MissingFileName));
        }
        catch (FileNotFoundException exception)
        {
            Console.WriteLine($"not found: {Path.GetFileName(exception.FileName)}");
        }

        // With Task.WhenAll, await only throws the FIRST exception...
        Task firstRead = File.ReadAllTextAsync(Path.Combine(workingDirectory, MissingFileName));
        Task secondRead = File.ReadAllTextAsync(Path.Combine(workingDirectory, OtherMissingFileName));
        Task bothReads = Task.WhenAll(firstRead, secondRead);

        try
        {
            await bothReads;
        }
        catch (FileNotFoundException)
        {
            // ...while the task itself collects every error in an AggregateException.
            AggregateException aggregateException = bothReads.Exception!;
            Console.WriteLine($"{aggregateException.InnerExceptions.Count} of 2 reads failed");
        }
    }

    // ---------------------------------------------------------------------
    // 7. Use case: results that arrive piece by piece (IAsyncEnumerable)
    // ---------------------------------------------------------------------

    private const string LogFileName = "application.log";
    private const string ErrorMarker = "ERROR";
    private const int LogLineCount = 50_000;
    private const int ErrorEveryNthLine = 7_500;
    private const int InterestingErrorCount = 3;

    private const string MeasurementsFileName = "measurements.json";
    private const int MeasurementCount = 10_000;
    private const double MeasurementBaseValue = 20.0;
    private const double MeasurementStep = 0.01;

    private static async Task StreamingAsync(string workingDirectory)
    {
        string logPath = Path.Combine(workingDirectory, LogFileName);

        // await foreach consumes the lines as they arrive. The 50 000 lines are never
        // in memory as a whole - File.ReadAllLinesAsync would do exactly that.
        // ReadErrorsAsync filters the stream, so the caller sees only the errors, and
        // because the values are pulled lazily, break really stops the reading.
        List<string> firstErrors = [];

        await foreach (string error in ReadErrorsAsync(logPath))
        {
            firstErrors.Add(error);

            if (firstErrors.Count == InterestingErrorCount)
            {
                break;
            }
        }

        Console.WriteLine($"{firstErrors.Count} errors read, last one: {firstErrors[^1]}");

        // The same idea for JSON: SerializeAsync writes into the stream instead of
        // building a huge string in memory first...
        string measurementsPath = Path.Combine(workingDirectory, MeasurementsFileName);

        IEnumerable<Measurement> measurements = Enumerable
            .Range(0, MeasurementCount)
            .Select(index => new Measurement($"sensor-{index % Repositories.Length}", MeasurementBaseValue + index * MeasurementStep));

        await using (FileStream writeStream = OpenForWriting(measurementsPath))
        {
            await JsonSerializer.SerializeAsync(writeStream, measurements);
        }

        // ...and DeserializeAsyncEnumerable yields the elements while the file is still
        // being read, instead of parsing the whole array first.
        await using FileStream readStream = OpenForReading(measurementsPath);

        double maximum = double.MinValue;
        int count = 0;

        await foreach (Measurement? measurement in JsonSerializer.DeserializeAsyncEnumerable<Measurement>(readStream))
        {
            if (measurement is null)
            {
                continue;
            }

            count++;
            maximum = Math.Max(maximum, measurement.Value);
        }

        Console.WriteLine($"{count} measurements streamed, maximum {maximum:F2}");
    }

    // async + IAsyncEnumerable<T> + yield return = asynchronous iterator.
    // [EnumeratorCancellation] connects a token from WithCancellation to the parameter.
    private static async IAsyncEnumerable<string> ReadErrorsAsync(
        string logPath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (string line in File.ReadLinesAsync(logPath, cancellationToken))
        {
            if (line.Contains(ErrorMarker, StringComparison.Ordinal))
            {
                yield return line;
            }
        }
    }

    // ---------------------------------------------------------------------
    // 8. Use case: I/O asynchronous, calculation on the thread pool (Task.Run)
    // ---------------------------------------------------------------------

    private static async Task CpuBoundWorkAsync(string workingDirectory)
    {
        // Reading the file is I/O: await gives the thread back while the disk works.
        string[] lines = await File.ReadAllLinesAsync(Path.Combine(workingDirectory, LogFileName));

        // Counting words in 50 000 lines is not I/O, it is CPU work - and await alone
        // creates no thread for it. Task.Run hands it to the thread pool, which in a
        // desktop application is what keeps the UI responsive. (In a web API it is the
        // opposite: there the request already runs on a pool thread.)
        int wordCount = await Task.Run(() => lines.Sum(line => line.Split(' ').Length));

        Console.WriteLine($"{lines.Length} lines, {wordCount} words");
    }

    // ---------------------------------------------------------------------
    // 9. Use case: progress of a long copy (IProgress<T>)
    // ---------------------------------------------------------------------

    private const string LogCopyFileName = "application.log.bak";
    private const int CopyBufferSize = 64 * 1024;
    private const int PercentFactor = 100;
    private const int ProgressStepPercent = 25;

    private static async Task ProgressReportingAsync(string workingDirectory)
    {
        // The copy routine only knows IProgress<T> and does not care whether the value
        // ends up in a progress bar, a log or a test.
        // Progress<T> would marshal the callback back to the captured context (the UI
        // thread in a desktop app); a console app has no such context, so the callbacks
        // would arrive on the thread pool - hence the synchronous implementation here.
        IProgress<int> progress = new SynchronousProgress(percent => Console.WriteLine($"{percent} % copied"));

        long copiedBytes = await CopyWithProgressAsync(
            Path.Combine(workingDirectory, LogFileName),
            Path.Combine(workingDirectory, LogCopyFileName),
            progress);

        Console.WriteLine($"{copiedBytes} bytes copied");
    }

    private static async Task<long> CopyWithProgressAsync(string sourcePath, string destinationPath, IProgress<int> progress)
    {
        await using FileStream source = OpenForReading(sourcePath);
        await using FileStream destination = OpenForWriting(destinationPath);

        // Without progress this would be one line: await source.CopyToAsync(destination).
        // The manual loop over ReadAsync/WriteAsync is what makes the progress possible.
        byte[] buffer = new byte[CopyBufferSize];
        long copiedBytes = 0;
        int reportedPercent = 0;

        while (await source.ReadAsync(buffer) is var readBytes && readBytes > 0)
        {
            await destination.WriteAsync(buffer.AsMemory(0, readBytes));
            copiedBytes += readBytes;

            int percent = (int)(copiedBytes * PercentFactor / source.Length);

            if (percent >= reportedPercent + ProgressStepPercent)
            {
                reportedPercent = percent;
                progress.Report(percent);
            }
        }

        progress.Report(PercentFactor); // the last chunk rarely lands exactly on 100 %

        return copiedBytes;
    }

    // ---------------------------------------------------------------------
    // 10. Use case: cached configuration without a Task allocation (ValueTask)
    // ---------------------------------------------------------------------

    private const string ConfigurationFileName = "settings.json";
    private const string ConfigurationContent = """{ "theme": "dark", "language": "de-CH" }""";

    private static readonly Dictionary<string, string> ConfigurationCache = [];

    private static async Task CachedValueTaskAsync(string workingDirectory)
    {
        string configurationPath = Path.Combine(workingDirectory, ConfigurationFileName);
        await File.WriteAllTextAsync(configurationPath, ConfigurationContent);

        // First call: really asynchronous, the file is read from disk.
        string firstRead = await ReadConfigurationAsync(configurationPath);

        // Second call: answered from the cache, so the method finishes synchronously.
        // ValueTask avoids the Task allocation for exactly this case - worth it on hot
        // paths that usually hit the cache, not as a default return type.
        string secondRead = await ReadConfigurationAsync(configurationPath);

        Console.WriteLine($"configuration cached: {ReferenceEquals(firstRead, secondRead)}");

        // Rule for ValueTask: await it exactly once and do not store it. Everything else
        // (WhenAll, several awaits) needs a real Task - AsTask() converts it.
        Task<string> asTask = ReadConfigurationAsync(configurationPath).AsTask();

        Console.WriteLine(await asTask == ConfigurationContent);
    }

    private static async ValueTask<string> ReadConfigurationAsync(string configurationPath)
    {
        if (ConfigurationCache.TryGetValue(configurationPath, out string? cachedContent))
        {
            return cachedContent; // no await: the ValueTask is already completed here
        }

        string content = await File.ReadAllTextAsync(configurationPath);
        ConfigurationCache[configurationPath] = content;

        return content;
    }

    // ---------------------------------------------------------------------
    // Pitfalls, in short:
    // - "async void" only for event handlers: the caller cannot await it, and an
    //   exception in it tears down the process instead of landing in a catch block.
    // - Never .Result or .Wait() on an async method: in a UI or older ASP.NET context
    //   this deadlocks, everywhere else it blocks a thread for nothing. Await instead.
    // - In library code ConfigureAwait(false) after the await, so the continuation does
    //   not need the caller's context.
    // - Pass the CancellationToken down through every layer instead of swallowing it.
    // ---------------------------------------------------------------------

    // ---------------------------------------------------------------------
    // Sample data and helpers
    // ---------------------------------------------------------------------

    private static readonly string[] ReportFileNames = ["report-q1.txt", "report-q2.txt", "report-q3.txt"];

    // FileOptions.Asynchronous tells the operating system that this handle is used
    // asynchronously - without it, ReadAsync/WriteAsync fall back to blocking calls.
    private static FileStream OpenForReading(string path)
    {
        return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, StreamBufferSize, FileOptions.Asynchronous);
    }

    private static FileStream OpenForWriting(string path)
    {
        return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, StreamBufferSize, FileOptions.Asynchronous);
    }

    private static string CreateWorkingDirectory()
    {
        string workingDirectory = Path.Combine(Path.GetTempPath(), WorkingDirectoryName);

        if (Directory.Exists(workingDirectory))
        {
            Directory.Delete(workingDirectory, recursive: true);
        }

        Directory.CreateDirectory(workingDirectory);

        return workingDirectory;
    }

    private static async Task PrepareSampleFilesAsync(string workingDirectory)
    {
        await using StreamWriter writer = new(Path.Combine(workingDirectory, LogFileName));

        for (int lineNumber = 1; lineNumber <= LogLineCount; lineNumber++)
        {
            string level = lineNumber % ErrorEveryNthLine == 0 ? ErrorMarker : "INFO";
            await writer.WriteLineAsync($"{level} line {lineNumber}: request handled");
        }

        IEnumerable<Task> reportTasks = ReportFileNames.Select((fileName, index) =>
            File.WriteAllTextAsync(Path.Combine(workingDirectory, fileName), $"quarter {index + 1}: revenue {index + 1}00"));

        await Task.WhenAll(reportTasks);
    }
}

// Deserialized from the GitHub API: JsonPropertyName maps the snake_case names of the
// JSON to the C# properties.
internal sealed record GitHubRepository(
    [property: JsonPropertyName("full_name")] string FullName,
    [property: JsonPropertyName("stargazers_count")] int StarCount);

internal sealed record Measurement(string Sensor, double Value);

// Minimal IProgress<T> implementation that calls back on the reporting thread.
internal sealed class SynchronousProgress : IProgress<int>
{
    private readonly Action<int> callback;

    public SynchronousProgress(Action<int> callback)
    {
        this.callback = callback;
    }

    public void Report(int value)
    {
        callback(value);
    }
}
