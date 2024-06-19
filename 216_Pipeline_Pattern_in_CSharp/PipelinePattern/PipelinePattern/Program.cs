using System;
using System.Runtime.Intrinsics.X86;

namespace PipelinePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var pipeline = new Pipeline<string>()
                .AddStep(new Step1())
            .AddStep(new Step2())
                .AddStep(new Step3());

            string input = "hello world";
            string result = pipeline.Execute(input);

            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Output: {result}");

            Console.ReadLine();
        }
    }
    public class Step1 : IPipelineStep<string>
    {
        public string Process(string input)
        {
            return input.ToUpper();
        }
    }

    public class Step2 : IPipelineStep<string>
    {
        public string Process(string input)
        {
            return input.Replace(" ", "_");
        }
    }

    public class Step3 : IPipelineStep<string>
    {
        public string Process(string input)
        {
            return input + "!";
        }
    }
}
