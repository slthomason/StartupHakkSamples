using System.Text;
using System.Text.Json;

namespace Simple_RAG_NET.Services;

public class OllamaService
{
    private readonly string _baseUrl;
    private readonly string _model;
    
    public OllamaService(string baseUrl, string model)
    {
        _baseUrl = baseUrl;
        _model = model;
    }
    
    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        using var client = new HttpClient();
        var request = new { model = _model, prompt = text };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync($"{_baseUrl}/api/embeddings", content);
        
        if (!response.IsSuccessStatusCode)
            throw new Exception($"API error: {await response.Content.ReadAsStringAsync()}");
        
        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<EmbeddingResponse>(responseJson, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
        return result?.Embedding ?? throw new Exception("No embedding received");
    }
    
    public async Task<string> GetCompletionAsync(string prompt)
    {
        using var client = new HttpClient();
        var request = new { model = _model, prompt, stream = false };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync($"{_baseUrl}/api/generate", content);
        
        if (!response.IsSuccessStatusCode)
            throw new Exception($"API error: {await response.Content.ReadAsStringAsync()}");
        
        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CompletionResponse>(responseJson,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
        return result?.Response ?? "No response";
    }
}

public class EmbeddingResponse
{
    public float[] Embedding { get; set; } = Array.Empty<float>();
}

public class CompletionResponse
{
    public string Response { get; set; } = string.Empty;
} 