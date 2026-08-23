using System.Numerics;

namespace LanguageFeaturesCSharp;

internal static class Simd
{
    public static void Show()
    {
        float[] a = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        float[] b = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160 };
        float[] sum = new float[a.Length];

        // Vector<float>.Count: how many float values the current hardware can process
        // simultaneously in a single SIMD register (depends on the processor).
        int width = Vector<float>.Count;

        Console.WriteLine(width);

        Vector<float> dotProductSum = Vector<float>.Zero;

        int i = 0;
        for (; i <= a.Length - width; i += width)
        {
            // Vector<float> loads "width" values at once from the array
            Vector<float> va = new Vector<float>(a, i);
            Vector<float> vb = new Vector<float>(b, i);

            // A single SIMD instruction adds all "width" values at once,
            // instead of adding them one by one in a loop.
            Vector<float> vSum = va + vb;
            vSum.CopyTo(sum, i);

            dotProductSum += va * vb;
        }

        // Remainder that no longer fits a full SIMD width is processed normally (scalar)
        float dotProductRemainder = 0f;
        for (; i < a.Length; i++)
        {
            sum[i] = a[i] + b[i];
            dotProductRemainder += a[i] * b[i];
        }

        // Vector.Sum: adds the individual values inside a Vector<float> into a scalar
        float dotProduct = Vector.Sum(dotProductSum) + dotProductRemainder;

        Console.WriteLine(string.Join(", ", sum));
        Console.WriteLine(dotProduct);

        // Whether SIMD is actually accelerated by the hardware (instead of just emulated) depends on the processor
        bool hardwareAccelerated = Vector.IsHardwareAccelerated;

        Console.WriteLine(hardwareAccelerated);
    }
}
