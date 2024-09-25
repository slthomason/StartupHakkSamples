using System;

namespace Prallelism;

using System;
using System.Threading.Tasks;

public static class DataParallelism
{
    public static void Main()
    {
        int[] numbers = new int[100];
        
        // Fill the array with numbers
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i;
        }

        // Perform a data parallel operation using Parallel.For
        Parallel.For(0, numbers.Length, i =>
        {
            numbers[i] *= 2; // Example operation: double each number
        });

        // Print the results
        Console.WriteLine(string.Join(", ", numbers));
    }
}
