using System;
using OpenAI.Chat;

namespace OpenAIConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter your OpenAI API key:");
            string apiKey = Console.ReadLine();

            // Initialize the OpenAI client

            ChatClient client = new(model: "gpt-3.5-turbo", Environment.GetEnvironmentVariable(apiKey));

            // Example input for natural language processing
            Console.WriteLine("Enter a prompt for the AI:");
            string prompt = Console.ReadLine();
            List<ChatMessage> messages = [
                    new UserChatMessage(prompt),
            ];

            ChatCompletion chatCompletion = client.CompleteChat(messages);

            Console.WriteLine("AI Response:");
            Console.WriteLine(chatCompletion);
        }
    }
}
