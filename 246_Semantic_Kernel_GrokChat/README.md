# Semantic Kernel Multi-Model Chat Application

This is a console application that demonstrates the integration of Microsoft's Semantic Kernel with X.AI Grok to create a flexible chat interface. The application streams responses in real-time as they are generated, providing a responsive user experience.

## Features

- Real-time streaming of AI responses
- Integration with X.AI for Grok inference
- Simple console-based chat interface
- Optimized for performance with asynchronous streaming
- Built on .NET 8 and Semantic Kernel 1.32.0

## Project Structure

- `Program.cs` - Main application entry point that sets up the Semantic Kernel, initializes services, and handles the chat loop
- `Services/XAIChatCompletionService.cs` - Implementation of the IChatCompletionService interface that connects to X.AI API
- `appsettings.json` - Configuration file containing API keys and endpoints
- `SemanticKernel.MultiModelChat.csproj` - Project file with dependencies

## Requirements

- .NET 8.0 SDK
- X.AI API key (stored in appsettings.json)

## Setup

1. Make sure your X.AI API key is correctly set in appsettings.json:
   ```json
   {
     "XAI": {
       "ApiKey": "your-api-key",
       "Endpoint": "https://api.x.ai/v1/chat/completions",
       "Model": "grok-1"
     }
   }
   ```

2. Build and run the application:
   ```
   dotnet build
   dotnet run
   ```

## How It Works

The application uses Microsoft's Semantic Kernel framework to create a chat interface that connects to X.AI for Grok LLM inference. Here's how it works:

1. The application initializes a Semantic Kernel instance and registers the XAIChatCompletionService.
2. A chat history is created with a system message that sets the assistant's behavior.
3. The main loop prompts the user for input and sends it to the LLM via the chat service.
4. Responses are streamed in real-time as they're generated, providing immediate feedback.
5. Both user inputs and assistant responses are added to the chat history to maintain context.

## Implementation Details

### Dependency Injection

The application uses Semantic Kernel's dependency injection to register services:

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var builder = Kernel.CreateBuilder();
builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddScoped<IChatCompletionService, XAIChatCompletionService>();
var kernel = builder.Build();
```

### Streaming Responses

The application uses asynchronous streaming to display responses as they're generated:

```csharp
await foreach (var partialResponse in chatService.GetStreamingChatMessageContentsAsync(history))
{
    Console.Write(partialResponse.Content);
    fullResponse.Append(partialResponse.Content);
}
```

### Chat History Management

The application maintains a chat history to provide context for the conversation:

```csharp
var history = new ChatHistory();
history.AddSystemMessage("You are a helpful assistant that will help with questions.");
// Later in the loop:
history.AddUserMessage(userMessage);
history.AddAssistantMessage(fullResponse.ToString());
```

## Dependencies

- Microsoft.SemanticKernel (1.32.0)
- Microsoft.SemanticKernel.Connectors.OpenAI (1.32.0)
- Microsoft.Extensions.Configuration.Json (8.0.0)
