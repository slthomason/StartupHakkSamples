using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Simple_RAG_NET.Services;

/// <summary>
/// Enhanced FAISS Vector Store with local caching and persistence support
/// </summary>
public class FaissVectorStore
{
    private readonly string _faissUrl;
    private readonly string _cacheFile;
    private readonly List<string> _documents = new();
    private readonly List<float[]> _embeddings = new();
    private bool _indexCreated = false;
    
    public FaissVectorStore(string faissUrl, string cacheFile = "vector_cache.json")
    {
        _faissUrl = faissUrl;
        _cacheFile = cacheFile;
        LoadCacheFromDisk();
    }
    
    public async Task AddDocumentAsync(string content, float[] embedding)
    {
        _documents.Add(content);
        _embeddings.Add(embedding);
        
        // Create or recreate the index with all documents
        await CreateIndexAsync(_embeddings, _documents);
        _indexCreated = true;
        
        // Save cache to disk
        SaveCacheToDisk();
        
        Console.WriteLine($"Updated FAISS index with document: {content.Substring(0, Math.Min(30, content.Length))}...");
    }
    
    public async Task AddDocumentsBatchAsync(List<(string content, float[] embedding)> documentsWithEmbeddings)
    {
        foreach (var (content, embedding) in documentsWithEmbeddings)
        {
            _documents.Add(content);
            _embeddings.Add(embedding);
        }
        
        await CreateIndexAsync(_embeddings, _documents);
        _indexCreated = true;
        
        // Save cache to disk
        SaveCacheToDisk();
        
        Console.WriteLine($"Created FAISS index with {documentsWithEmbeddings.Count} documents");
    }
    
    /// <summary>
    /// Load existing documents and embeddings from cache, rebuild FAISS index
    /// </summary>
    public async Task<bool> LoadFromCacheAsync()
    {
        if (_documents.Any() && _embeddings.Any())
        {
            try
            {
                await CreateIndexAsync(_embeddings, _documents);
                _indexCreated = true;
                Console.WriteLine($"Restored FAISS index from cache with {_documents.Count} documents");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to restore FAISS index: {ex.Message}");
                return false;
            }
        }
        
        Console.WriteLine("No cache found or cache is empty");
        return false;
    }
    
    public async Task<List<string>> SearchSimilarAsync(float[] queryEmbedding, int topK = 3)
    {
        if (!_indexCreated || !_documents.Any())
        {
            Console.WriteLine("No documents in FAISS vector store!");
            return new List<string> { "No relevant documents found." };
        }
        
        try
        {
            using var client = new HttpClient();
            var request = new QueryRequest
            {
                Query = queryEmbedding.ToList(),
                K = Math.Min(topK, _documents.Count)
            };
            
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync($"{_faissUrl}/query", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"FAISS API error: {error}");
            }
            
            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<QueryResponseEnhanced>(responseJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            if (result?.Indices == null || !result.Indices.Any())
            {
                return new List<string> { "No relevant documents found." };
            }
            
            var relevantDocs = new List<string>();
            
            Console.WriteLine($"Found {result.Indices.Count} documents from FAISS:");
            for (int i = 0; i < result.Indices.Count && i < result.Distances.Count; i++)
            {
                var index = result.Indices[i];
                var distance = result.Distances[i];
                
                if (index >= 0 && index < _documents.Count)
                {
                    var doc = _documents[index];
                    Console.WriteLine($"  - Distance {distance:F3}: {doc.Substring(0, Math.Min(50, doc.Length))}...");
                    
                    // Convert L2 distance to similarity (lower distance = higher similarity)
                    if (distance < 300.0f) // Adjusted for 1024-dimensional embeddings
                    {
                        relevantDocs.Add(doc);
                    }
                }
            }
            
            Console.WriteLine($"Returning {relevantDocs.Count} documents above similarity threshold");
            
            return relevantDocs.Any() ? relevantDocs : new List<string> { "No relevant documents found." };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error querying FAISS: {ex.Message}");
            return new List<string> { "Error retrieving documents." };
        }
    }
    
    /// <summary>
    /// Save cache and trigger FAISS service to save to disk
    /// </summary>
    public async Task SaveAsync()
    {
        SaveCacheToDisk();
        
        try
        {
            using var client = new HttpClient();
            var response = await client.PostAsync($"{_faissUrl}/save", null);
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("FAISS service saved to disk");
            }
            else
            {
                Console.WriteLine($"Failed to trigger FAISS save: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to trigger FAISS save: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Clear all data and caches
    /// </summary>
    public async Task ClearAsync()
    {
        _documents.Clear();
        _embeddings.Clear();
        _indexCreated = false;
        
        // Delete local cache
        if (File.Exists(_cacheFile))
        {
            File.Delete(_cacheFile);
            Console.WriteLine($"Deleted local cache: {_cacheFile}");
        }
        
        // Clear FAISS service
        try
        {
            using var client = new HttpClient();
            var response = await client.DeleteAsync($"{_faissUrl}/clear");
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("FAISS service data cleared");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to clear FAISS service: {ex.Message}");
        }
    }
    
    private async Task CreateIndexAsync(List<float[]> vectors, List<string> documents)
    {
        using var client = new HttpClient();
        var request = new IndexRequestEnhanced 
        { 
            Vectors = vectors.Select(v => v.ToList()).ToList(),
            Documents = documents
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync($"{_faissUrl}/index", content);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create FAISS index: {error}");
        }
        
        var responseJson = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"FAISS index created: {responseJson}");
    }
    
    private void SaveCacheToDisk()
    {
        try
        {
            var cache = new VectorCache
            {
                Documents = _documents,
                Embeddings = _embeddings.Select(e => e.ToList()).ToList(),
                CreatedAt = DateTime.UtcNow
            };
            
            var json = JsonSerializer.Serialize(cache, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            File.WriteAllText(_cacheFile, json);
            Console.WriteLine($"Cache saved to {_cacheFile} ({_documents.Count} documents)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save cache: {ex.Message}");
        }
    }
    
    private void LoadCacheFromDisk()
    {
        try
        {
            if (File.Exists(_cacheFile))
            {
                var json = File.ReadAllText(_cacheFile);
                var cache = JsonSerializer.Deserialize<VectorCache>(json);
                
                if (cache != null)
                {
                    _documents.Clear();
                    _embeddings.Clear();
                    
                    _documents.AddRange(cache.Documents);
                    _embeddings.AddRange(cache.Embeddings.Select(e => e.ToArray()));
                    
                    Console.WriteLine($"Cache loaded from {_cacheFile} ({_documents.Count} documents, created: {cache.CreatedAt})");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load cache: {ex.Message}");
        }
    }
    
    public int DocumentCount => _documents.Count;
    public bool HasCache => File.Exists(_cacheFile);
    public DateTime? CacheCreatedAt 
    {
        get
        {
            try
            {
                if (File.Exists(_cacheFile))
                {
                    var json = File.ReadAllText(_cacheFile);
                    var cache = JsonSerializer.Deserialize<VectorCache>(json);
                    return cache?.CreatedAt;
                }
            }
            catch { }
            return null;
        }
    }
}

public class IndexRequestEnhanced
{
    [JsonPropertyName("vectors")]
    public List<List<float>> Vectors { get; set; } = new();
    
    [JsonPropertyName("documents")]
    public List<string> Documents { get; set; } = new();
}

public class QueryRequest
{
    [JsonPropertyName("query")]
    public List<float> Query { get; set; } = new();
    
    [JsonPropertyName("k")]
    public int K { get; set; } = 5;
}

public class QueryResponseEnhanced
{
    [JsonPropertyName("indices")]
    public List<int> Indices { get; set; } = new();
    
    [JsonPropertyName("distances")]
    public List<float> Distances { get; set; } = new();
    
    [JsonPropertyName("documents")]
    public List<string> Documents { get; set; } = new();
}

public class VectorCache
{
    [JsonPropertyName("documents")]
    public List<string> Documents { get; set; } = new();
    
    [JsonPropertyName("embeddings")]
    public List<List<float>> Embeddings { get; set; } = new();
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}
