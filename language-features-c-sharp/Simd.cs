using System.Numerics;

namespace Language_Features_C_Sharp;

internal static class Simd
{
    public static void Zeigen()
    {
        float[] a = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        float[] b = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160 };
        float[] summe = new float[a.Length];

        // Vector<float>.Count: wie viele float-Werte die aktuelle Hardware gleichzeitig
        // in einem einzigen SIMD-Register verarbeiten kann (haengt vom Prozessor ab).
        int breite = Vector<float>.Count;

        Console.WriteLine(breite);

        Vector<float> skalarproduktSumme = Vector<float>.Zero;

        int i = 0;
        for (; i <= a.Length - breite; i += breite)
        {
            // Vector<float> laedt "breite" Werte auf einmal aus dem Array
            Vector<float> va = new Vector<float>(a, i);
            Vector<float> vb = new Vector<float>(b, i);

            // Ein einziger SIMD-Befehl addiert alle "breite" Werte gleichzeitig,
            // statt sie einzeln in einer Schleife zu addieren.
            Vector<float> vSumme = va + vb;
            vSumme.CopyTo(summe, i);

            skalarproduktSumme += va * vb;
        }

        // Rest, der nicht mehr in eine volle SIMD-Breite passt, wird normal (skalar) verarbeitet
        float skalarproduktRest = 0f;
        for (; i < a.Length; i++)
        {
            summe[i] = a[i] + b[i];
            skalarproduktRest += a[i] * b[i];
        }

        // Vector.Sum: addiert die einzelnen Werte innerhalb eines Vector<float> zu einem Skalar
        float skalarprodukt = Vector.Sum(skalarproduktSumme) + skalarproduktRest;

        Console.WriteLine(string.Join(", ", summe));
        Console.WriteLine(skalarprodukt);

        // Ob SIMD tatsaechlich von der Hardware beschleunigt wird (statt nur emuliert), haengt vom Prozessor ab
        bool hardwareBeschleunigt = Vector.IsHardwareAccelerated;

        Console.WriteLine(hardwareBeschleunigt);
    }
}
