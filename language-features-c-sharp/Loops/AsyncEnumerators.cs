using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageFeaturesCSharp.Loops;

// The asynchronous counterpart of IEnumerable<T> (see Enumerators.cs): the collection hands out
// a cursor whose "move on" step has to be awaited, because the next element may not exist yet.
internal class SensorReadings : IAsyncEnumerable<int>
{
    // Stands for the real waiting time of a measurement
    internal const int MeasurementDelayMilliseconds = 20;

    private readonly int readingCount;

    public SensorReadings(int readingCount)
    {
        this.readingCount = readingCount;
    }

    // Unlike GetEnumerator this method takes a cancellation token: an asynchronous loop can
    // run for a long time, so it must be possible to stop it from the outside.
    public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new SensorReadingsEnumerator(readingCount, MeasurementDelayMilliseconds, cancellationToken);
    }
}

internal class SensorReadingsEnumerator : IAsyncEnumerator<int>
{
    private readonly int readingCount;

    private readonly int measurementDelayMilliseconds;

    private readonly CancellationToken cancellationToken;

    private int position;

    public SensorReadingsEnumerator(int readingCount, int measurementDelayMilliseconds, CancellationToken cancellationToken)
    {
        this.readingCount = readingCount;
        this.measurementDelayMilliseconds = measurementDelayMilliseconds;
        this.cancellationToken = cancellationToken;
    }

    // Current is a plain property again: only moving on is asynchronous, reading is not
    public int Current { get; private set; }

    // MoveNextAsync returns ValueTask<bool> instead of bool. ValueTask instead of Task, because
    // the value is often already there and then no Task object has to be allocated at all.
    public async ValueTask<bool> MoveNextAsync()
    {
        if (position >= readingCount)
        {
            return false;
        }

        // Here stands the real waiting: a sensor, a network call, a database cursor
        await Task.Delay(measurementDelayMilliseconds, cancellationToken);

        position++;
        Current = position * 10;

        return true;
    }

    // DisposeAsync instead of Dispose: closing a connection can itself be asynchronous.
    // await foreach calls it at the end, just like foreach calls Dispose.
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}

internal static class AsyncEnumerators
{
    private const int ReadingCount = 3;

    private const int CancellationThreshold = 30;

    private const int MeasurementDelayMilliseconds = SensorReadings.MeasurementDelayMilliseconds;

    private const string WorkingDirectoryName = "language-features-async-streams";
    private const int StreamBufferSize = 4096;

    private const string LogFileName = "application.log";
    private const string ErrorMarker = "ERROR";
    private const int LogLineCount = 50_000;
    private const int ErrorEveryNthLine = 7_500;
    private const int InterestingErrorCount = 3;

    private const string MeasurementsFileName = "measurements.json";
    private const int MeasurementCount = 10_000;
    private const int SensorCount = 3;
    private const double MeasurementBaseValue = 20.0;
    private const double MeasurementStep = 0.01;

    public static async Task ShowAsync()
    {
        SensorReadings readings = new SensorReadings(ReadingCount);

        // await foreach awaits every single step of the loop. The elements arrive one by one,
        // and the thread is free in between instead of blocking.
        await foreach (int reading in readings)
        {
            Console.WriteLine(reading);
        }

        // Written out, await foreach is exactly this - the asynchronous twin of the loop
        // in Enumerators.cs, with await in front of MoveNextAsync and DisposeAsync.
        IAsyncEnumerator<int> enumerator = readings.GetAsyncEnumerator();

        try
        {
            while (await enumerator.MoveNextAsync())
            {
                int reading = enumerator.Current;

                Console.WriteLine(reading);
            }
        }
        finally
        {
            await enumerator.DisposeAsync();
        }

        // The alternative without IAsyncEnumerable is Task<IEnumerable<T>>: there the caller
        // waits for the LAST element before it gets to see the first one.
        IEnumerable<int> allReadingsAtOnce = await LoadAllReadingsAsync(ReadingCount);

        Console.WriteLine(string.Join(", ", allReadingsAtOnce));

        // Implementing the interface by hand is rarely needed: async + IAsyncEnumerable<T> +
        // yield return builds the same state machine automatically.
        await foreach (int reading in GeneratedReadingsAsync(ReadingCount))
        {
            Console.WriteLine(reading);
        }

        // Like a normal iterator, an async iterator is lazy: calling the method starts nothing,
        // the first element is produced only when the loop asks for it.
        IAsyncEnumerable<int> notStartedYet = GeneratedReadingsAsync(ReadingCount);

        Console.WriteLine("nothing measured so far");

        int firstReading = 0;

        await foreach (int reading in notStartedYet)
        {
            firstReading = reading;

            break; // break also ends the underlying iterator properly, via DisposeAsync
        }

        Console.WriteLine(firstReading);

        // Cancelling: WithCancellation passes the token to the [EnumeratorCancellation]
        // parameter of the iterator, so the wait inside the loop is aborted too.
        using CancellationTokenSource cancellation = new CancellationTokenSource();

        try
        {
            await foreach (int reading in GeneratedReadingsAsync(int.MaxValue).WithCancellation(cancellation.Token))
            {
                Console.WriteLine(reading);

                if (reading >= CancellationThreshold)
                {
                    cancellation.Cancel();
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("measurement cancelled");
        }

        // Since .NET 10 the LINQ methods also exist for async streams, with an Async suffix
        // where they have to await a result. Before that this needed a separate NuGet package.
        int largeReadingCount = await GeneratedReadingsAsync(ReadingCount).Where(reading => reading > 10).CountAsync();
        List<int> collectedReadings = await GeneratedReadingsAsync(ReadingCount).Select(reading => reading / 10).ToListAsync();

        Console.WriteLine(largeReadingCount);
        Console.WriteLine(string.Join(", ", collectedReadings));

        await StreamingAsync();
    }

    // ---------------------------------------------------------------------
    // Where it pays off: data that does not fit into memory as a whole
    // ---------------------------------------------------------------------

    private static async Task StreamingAsync()
    {
        string workingDirectory = CreateWorkingDirectory();

        try
        {
            await WriteSampleLogAsync(workingDirectory);

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
                .Select(index => new Measurement($"sensor-{index % SensorCount}", MeasurementBaseValue + index * MeasurementStep));

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
        finally
        {
            Directory.Delete(workingDirectory, recursive: true);
        }
    }

    // The same asynchronous iterator as above, but over a real source: it filters a file
    // that is still being read, without ever holding all of it in memory.
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
    // Sample data and file helpers
    // ---------------------------------------------------------------------

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

    private static async Task WriteSampleLogAsync(string workingDirectory)
    {
        await using StreamWriter writer = new(Path.Combine(workingDirectory, LogFileName));

        for (int lineNumber = 1; lineNumber <= LogLineCount; lineNumber++)
        {
            string level = lineNumber % ErrorEveryNthLine == 0 ? ErrorMarker : "INFO";
            await writer.WriteLineAsync($"{level} line {lineNumber}: request handled");
        }
    }

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

    // async + IAsyncEnumerable<T> + yield return = asynchronous iterator.
    // [EnumeratorCancellation] is what connects the token from WithCancellation to this parameter.
    private static async IAsyncEnumerable<int> GeneratedReadingsAsync(
        int readingCount,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (int position = 1; position <= readingCount; position++)
        {
            await Task.Delay(MeasurementDelayMilliseconds, cancellationToken);

            yield return position * 10;
        }
    }

    // The same data, but collected completely before anything is returned
    private static async Task<IEnumerable<int>> LoadAllReadingsAsync(int readingCount)
    {
        List<int> readings = [];

        for (int position = 1; position <= readingCount; position++)
        {
            await Task.Delay(MeasurementDelayMilliseconds);

            readings.Add(position * 10);
        }

        return readings;
    }
}

// One measurement out of the JSON file: a record is enough, because it only carries data
internal sealed record Measurement(string Sensor, double Value);
