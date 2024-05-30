using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        const int iterations = 1000000;
        var arrayList = new ArrayList();
        var list = new List<int>();

        // Filling collections with elements
        for (int i = 0; i < iterations; i++)
        {
            arrayList.Add(i);
            list.Add(i);
        }

        var stopwatch = new Stopwatch();

        // Accessing elements by index in ArrayList
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            var value = (int)arrayList[i];
        }
        stopwatch.Stop();
        Console.WriteLine($"ArrayList: {stopwatch.ElapsedMilliseconds} ms");

        // Accessing elements by index in List<int>
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            var value = list[i];
        }
        stopwatch.Stop();
        Console.WriteLine($"List<int>: {stopwatch.ElapsedMilliseconds} ms");
    }
}