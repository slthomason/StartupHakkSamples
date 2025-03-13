using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;

var builder = Kernel.CreateBuilder();

builder.Services.AddScoped<IOllamaApiClient>(_ => new OllamaApiClient("http://localhost:11434"));

builder.Services.AddScoped<IChatCompletionService, OllamaChatCompletionService>();

var kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are a helpful assistant that will help with questions.");

while (true)
{
    Console.Write("You: ");
    var userMessage = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userMessage))
    {
        break;
    }

    history.AddUserMessage(userMessage);

    Console.Write("Bot: ");
    var fullResponse = new System.Text.StringBuilder();
    
    // streaming API to show responses incrementally
    await foreach (var partialResponse in chatService.GetStreamingChatMessageContentsAsync(history))
    {
        Console.Write(partialResponse.Content);
        fullResponse.Append(partialResponse.Content);
    }
    
    Console.WriteLine(); // Add a newline 
    
    // add response to history
    history.AddAssistantMessage(fullResponse.ToString());
}
