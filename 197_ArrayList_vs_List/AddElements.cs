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

        var stopwatch = new Stopwatch();

        // Adding elements to ArrayList
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            arrayList.Add(i);
        }
        stopwatch.Stop();
        Console.WriteLine($"ArrayList: {stopwatch.ElapsedMilliseconds} ms");

        // Adding elements to List<int>
        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            list.Add(i);
        }
        stopwatch.Stop();
        Console.WriteLine($"List<int>: {stopwatch.ElapsedMilliseconds} ms");
    }
}