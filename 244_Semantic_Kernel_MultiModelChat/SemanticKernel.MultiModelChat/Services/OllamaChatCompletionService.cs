using System.ClientModel.Primitives;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;
using OllamaSharp.Models.Chat;

public class OllamaChatCompletionService : IChatCompletionService
{
    private readonly IOllamaApiClient ollamaApiClient;
    private const string DefaultModel = "phi3:mini"; // model as a constant for easy changing

    public OllamaChatCompletionService(IOllamaApiClient ollamaApiClient)
    {
        this.ollamaApiClient = ollamaApiClient;
    }

    public IReadOnlyDictionary<string, object?> Attributes => new Dictionary<string, object?>();

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default
    )
    {
        // For non-streaming requests, we'll use the streaming API internally
        // and collect the results for better performance
        var content = new StringBuilder();
        AuthorRole authorRole = AuthorRole.Assistant;

        await foreach (var response in GetStreamingChatMessageContentsAsync(
            chatHistory, executionSettings, kernel, cancellationToken))
        {
            content.Append(response.Content);
            authorRole = (AuthorRole)response.Role;
        }

        return
        [
            new ChatMessageContent
            {
                Role = authorRole,
                Content = content.ToString(),
                ModelId = DefaultModel
            }
        ];
    }

    public async IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var request = CreateChatRequest(chatHistory);

        await foreach (var response in ollamaApiClient.ChatAsync(request, cancellationToken))
        {
            if (response?.Message?.Content == null)
            {
                continue; // skip empty responses
            }

            yield return new StreamingChatMessageContent(
                role: GetAuthorRole(response.Message.Role),
                content: response.Message.Content,
                innerContent: response,
                modelId: DefaultModel
            );
        }
    }

    private static AuthorRole GetAuthorRole(ChatRole? role)
    {
        return role?.ToString().ToUpperInvariant() switch
        {
            "USER" => AuthorRole.User,
            "ASSISTANT" => AuthorRole.Assistant,
            "SYSTEM" => AuthorRole.System,
            _ => AuthorRole.Assistant // Default to Assistant
        };
    }

    private static ChatRequest CreateChatRequest(ChatHistory chatHistory)
    {
        var messages = new List<Message>();

        foreach (var message in chatHistory)
        {
            ChatRole role;
            
            if (message.Role == AuthorRole.User)
                role = ChatRole.User;
            else if (message.Role == AuthorRole.System)
                role = ChatRole.System;
            else
                role = ChatRole.Assistant;

            messages.Add(
                new Message
                {
                    Role = role,
                    Content = message.Content,
                }
            );
        }

        return new ChatRequest
        {
            Messages = messages,
            Stream = true,
            Model = DefaultModel
        };
    }
}
