using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MCPServer.CSharp;

public class LlmServiceConfig
{
    public ApiKeySettings ApiKeys { get; set; } = new();
    public EndpointSettings Endpoints { get; set; } = new();

    public class ApiKeySettings
    {
        public string Grok { get; set; } = string.Empty;
    }

    public class EndpointSettings
    {
        public string Ollama { get; set; } = "http://localhost:11434";
    }
}

public class LlmService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LlmService> _logger;
    private readonly LlmServiceConfig _config;

    public LlmService(IHttpClientFactory httpClientFactory, ILogger<LlmService> logger, IOptions<LlmServiceConfig>? config = null)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        _config = config?.Value ?? new LlmServiceConfig
        {
            ApiKeys = new LlmServiceConfig.ApiKeySettings
            {
                Grok = ""
            },
            Endpoints = new LlmServiceConfig.EndpointSettings
            {
                Ollama = "http://localhost:11434"
            }
        };
    }

    /// <summary>
    /// Sends a prompt to Grok API and returns the response
    /// </summary>
    public async Task<string> CallGrokApiAsync(string prompt, string? apiKey = null, string model = "grok-2-1212")
    {
        try
        {
            // Use provided API key or the one from config
            apiKey ??= _config.ApiKeys.Grok;
            
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.x.ai/v1/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            
            var requestBody = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                stream = false
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("chat/completions", content);
            
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadFromJsonAsync<JsonElement>();
            return responseJson.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "No response";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Grok API");
            return $"Error: {ex.Message}";
        }
    }

    /// <summary>
    /// Sends a prompt to Ollama API and returns the response
    /// </summary>
    public async Task<string> CallOllamaApiAsync(string prompt, string model = "phi3:mini", string? baseUrl = null)
    {
        try
        {
            // Use provided base URL or the one from config
            baseUrl ??= _config.Endpoints.Ollama;
            
            _logger.LogInformation("Calling Ollama API at {BaseUrl} with model {Model}", baseUrl, model);
            
            var requestBody = new
            {
                model = model,
                prompt = prompt,
                stream = false
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/api/generate");
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadFromJsonAsync<JsonElement>();
            return responseJson.GetProperty("response").GetString() ?? "No response";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Ollama API at {BaseUrl} with model {Model}: {Message}", 
                baseUrl ?? _config.Endpoints.Ollama, model, ex.Message);
            return $"Error calling Ollama API: {ex.Message}";
        }
    }

    /// <summary>
    /// Gets a list of available models from Ollama
    /// </summary>
    public async Task<List<string>> GetOllamaModelsAsync(string? baseUrl = null)
    {
        try
        {
            // Use provided base URL or the one from config
            baseUrl ??= _config.Endpoints.Ollama;
            
            _logger.LogInformation("Getting available models from Ollama at {BaseUrl}", baseUrl);
            
            var response = await _httpClient.GetAsync($"{baseUrl}/api/tags");
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadFromJsonAsync<JsonElement>();
            var models = new List<string>();
            
            foreach (var model in responseJson.GetProperty("models").EnumerateArray())
            {
                models.Add(model.GetProperty("name").GetString() ?? "");
            }
            
            _logger.LogInformation("Found {Count} models on Ollama server", models.Count);
            return models;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Ollama models from {BaseUrl}: {Message}", 
                baseUrl ?? _config.Endpoints.Ollama, ex.Message);
            return new List<string>();
        }
    }
} 