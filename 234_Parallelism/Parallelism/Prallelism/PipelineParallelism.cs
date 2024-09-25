using System;

namespace Prallelism;

public static  class PipelineParallelism
{
    public static void Main()
    {
        var data = new[] { 1, 2, 3, 4, 5 };

        // First stage: Read data
        Task<int[]> readTask = Task.Run(() => ReadData(data));

        // Second stage: Process data
        Task<int[]> processTask = readTask.ContinueWith(t => ProcessData(t.Result));

        // Third stage: Write results
        processTask.ContinueWith(t => WriteData(t.Result)).Wait();
    }

    static int[] ReadData(int[] input)
    {
        Console.WriteLine("Reading data...");
        return input;
    }

    static int[] ProcessData(int[] input)
    {
        Console.WriteLine("Processing data...");
        for (int i = 0; i < input.Length; i++)
        {
            input[i] *= 10; // Example processing
        }
        return input;
    }

    static void WriteData(int[] output)
    {
        Console.WriteLine("Writing data...");
        Console.WriteLine(string.Join(", ", output));
    }
}
