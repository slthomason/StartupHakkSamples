using System;
using System.Collections.Generic;

namespace RoslynAnalyzerDemo
{
    // Example class for Roslyn.Diagnostics.Analyzers
    public class Reflection
    {
        public string Name { get; set; } = "Reflection";
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Example for Microsoft.CodeAnalysis.NetAnalyzers
            List<int> numbers = new() { 1, 2, 3, 4, 5 };

            for (int i = 0; i < 1000; i++)
            {
                if (numbers.Contains(i))
                {
                    Console.WriteLine(i);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
