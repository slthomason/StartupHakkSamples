using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;

namespace OpenAPIExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Replace with your OpenAI API key
            string apiKey = "YOUR_API_KEY_HERE";

            // Initialize the OpenAI API client
            var openai = new OpenAIAPI(apiKey);

            // Example 1: Create a completion
            await ExampleCreateCompletion(openai);

            // Example 2: Create a completion with parameters
            await ExampleCreateCompletionWithParameters(openai);

        }

        // Example 1: Create a completion
        static async Task ExampleCreateCompletion(OpenAIAPI openai)
        {
            var completionRequest = new CompletionRequest
            {
                Model = "text-davinci-003",
                Prompt = "Once upon a time",
                MaxTokens = 50
            };

            var completion = await openai.Completions.CreateCompletionAsync(completionRequest);
            Console.WriteLine("Example 1: Create a completion");
            Console.WriteLine(completion.ToString());
            Console.WriteLine();
        }

        // Example 2: Create a completion with parameters
        static async Task ExampleCreateCompletionWithParameters(OpenAIAPI openai)
        {
            var completionRequest = new CompletionRequest
            {
                Model = "text-davinci-003",
                Prompt = "Translate the following English text into French: Hello, how are you?",
                MaxTokens = 50,
                Temperature = 0.7,
                TopP = 1,
                FrequencyPenalty = 0.5,
                PresencePenalty = 0.0
            };

            var completion = await openai.Completions.CreateCompletionAsync(completionRequest);
            Console.WriteLine("Example 2: Create a completion with parameters");
            Console.WriteLine(completion.ToString());
            Console.WriteLine();
        }

      
    }
}
