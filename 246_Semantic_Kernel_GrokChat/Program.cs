using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

// Load configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

// Set up Kernel with X.AI service
var builder = Kernel.CreateBuilder();
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddScoped<IChatCompletionService, XAIChatCompletionService>();

var kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();
history.AddSystemMessage("You are a helpful assistant that will help with questions.");

Console.WriteLine("Chat with Grok. Type 'exit' to end the conversation.");

while (true)
{
    Console.Write("You: ");
    var userMessage = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userMessage) || userMessage.ToLower() == "exit")
    {
        break;
    }

    history.AddUserMessage(userMessage);

    Console.Write("Grok: ");
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
