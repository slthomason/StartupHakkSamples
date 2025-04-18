using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json.Nodes;

namespace MCPServer.CSharp;

[McpServerToolType]
public static class LlmTools
{
    [McpServerTool, Description("Sends a prompt to Grok-2 AI from X.AI and returns the response.")]
    public static async Task<string> AskGrok(
        LlmService llmService,
        [Description("The prompt to send to Grok")] string prompt,
        [Description("Your X.AI API key (optional if configured)")] string? apiKey = null,
        [Description("The model to use (default: grok-2-1212)")] string model = "grok-2-1212")
    {
        return await llmService.CallGrokApiAsync(prompt, apiKey, model);
    }

    [McpServerTool, Description("Sends a prompt to local Ollama API and returns the response using phi3:mini model.")]
    public static async Task<string> AskOllama(
        LlmService llmService,
        [Description("The prompt to send to Ollama")] string prompt,
        [Description("The model to use (default: phi3:mini)")] string model = "phi3:mini",
        [Description("The base URL of the Ollama API (optional if configured in appsettings.json)")] string? baseUrl = null)
    {
        return await llmService.CallOllamaApiAsync(prompt, model, baseUrl);
    }

    [McpServerTool, Description("Gets a list of available models from local Ollama server.")]
    public static async Task<string> ListOllamaModels(
        LlmService llmService,
        [Description("The base URL of the Ollama API (optional if configured in appsettings.json)")] string? baseUrl = null)
    {
        var models = await llmService.GetOllamaModelsAsync(baseUrl);
        return JsonSerializer.Serialize(models);
    }
} 