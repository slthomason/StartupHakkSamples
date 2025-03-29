using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public class XAIChatCompletionService : IChatCompletionService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint;
    private readonly string _model;

    public XAIChatCompletionService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = configuration["XAI:ApiKey"] ?? throw new ArgumentNullException("XAI:ApiKey not found in configuration");
        _endpoint = configuration["XAI:Endpoint"] ?? throw new ArgumentNullException("XAI:Endpoint not found in configuration");
        _model = configuration["XAI:Model"] ?? "grok-1";
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public IReadOnlyDictionary<string, object?> Attributes => new Dictionary<string, object?>();

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default
    )
    {
        var content = new StringBuilder();
        
        await foreach (var response in GetStreamingChatMessageContentsAsync(
            chatHistory, executionSettings, kernel, cancellationToken))
        {
            content.Append(response.Content);
        }

        return
        [
            new ChatMessageContent
            {
                Role = AuthorRole.Assistant,
                Content = content.ToString(),
                ModelId = _model
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
        var requestBody = new
        {
            model = _model,
            messages = chatHistory.Select(m => new 
            {
                role = m.Role.ToString().ToLower(),
                content = m.Content
            }).ToArray(),
            stream = true
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var request = new HttpRequestMessage(HttpMethod.Post, _endpoint)
        {
            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
        };

        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(cancellationToken) ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: "))
                continue;

            var data = line.Substring(6);
            
            if (data == "[DONE]")
                break;

            StreamResponse? streamResponse = null;
            string? content = null;
            
            try
            {
                streamResponse = JsonSerializer.Deserialize<StreamResponse>(data);
                content = streamResponse?.choices?[0]?.delta?.content;
            }
            catch
            {
                // Skip malformed JSON or other parsing errors
                continue;
            }
            
            if (content != null)
            {
                yield return new StreamingChatMessageContent(
                    role: AuthorRole.Assistant,
                    content: content,
                    modelId: _model
                );
            }
        }
    }
    
    private class StreamResponse
    {
        public Choice[]? choices { get; set; }
        
        public class Choice
        {
            public Delta? delta { get; set; }
        }
        
        public class Delta
        {
            public string? content { get; set; }
        }
    }
} 