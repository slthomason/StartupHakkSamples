# Semantic Kernel Multi-Model Chat Application

This is a console application that demonstrates the integration of Microsoft's Semantic Kernel with Ollama to create a flexible chat interface supporting multiple local LLM models. The application streams responses in real-time as they are generated, providing a responsive user experience.

## Features

- Real-time streaming of AI responses
- Integration with Ollama for local LLM inference
- Support for multiple models including Phi3, Grok, and Llama3
- Simple console-based chat interface
- Optimized for performance with asynchronous streaming
- Built on .NET 8 and Semantic Kernel 1.32.0

## Project Structure

- `Program.cs` - Main application entry point that sets up the Semantic Kernel, initializes services, and handles the chat loop
- `Services/OllamaChatCompletionService.cs` - Implementation of the IChatCompletionService interface that connects to Ollama
- `SemanticKernel.MultiModelChat.csproj` - Project file with dependencies

## Requirements

- .NET 8.0 SDK
- Ollama running locally on port 11434
- Models installed in Ollama:
  - Phi3 mini (default model)
  - Other models as needed (Grok, Llama3, etc.)

## Setup

1. Make sure Ollama is installed and running:
   ```
   ollama serve
   ```

2. Pull the required models (if not already installed):
   ```
   ollama pull phi3:mini
   ```

3. Build and run the application:
   ```
   dotnet build
   dotnet run
   ```

## How It Works

The application uses Microsoft's Semantic Kernel framework to create a chat interface that connects to Ollama for local LLM inference. Here's how it works:

1. The application initializes a Semantic Kernel instance and registers the OllamaChatCompletionService.
2. A chat history is created with a system message that sets the assistant's behavior.
3. The main loop prompts the user for input and sends it to the LLM via the chat service.
4. Responses are streamed in real-time as they're generated, providing immediate feedback.
5. Both user inputs and assistant responses are added to the chat history to maintain context.

## Implementation Details

### Dependency Injection

The application uses Semantic Kernel's dependency injection to register services:

```csharp
var builder = Kernel.CreateBuilder();
builder.Services.AddScoped<IOllamaApiClient>(_ => new OllamaApiClient("http://localhost:11434"));
builder.Services.AddScoped<IChatCompletionService, OllamaChatCompletionService>();
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

## Customization

You can customize the application in several ways:

1. **Change the default model**: Modify the `DefaultModel` constant in `OllamaChatCompletionService.cs`.
2. **Add model switching**: Implement commands to switch between different models.
3. **Modify system prompt**: Change the system message to alter the assistant's behavior.
4. **Add additional features**: Implement features like conversation saving/loading, context window management, etc.

## Dependencies

- Microsoft.SemanticKernel (1.32.0)
- OllamaSharp (4.0.11)
