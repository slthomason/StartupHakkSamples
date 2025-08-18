namespace Simple_RAG_NET.Services;

public class RagService
{
    private readonly OllamaService _ollama;
    private readonly FaissVectorStore _vectorStore;
    
    public RagService(string ollamaUrl, string embedModel, string completionModel, string faissUrl)
    {
        _ollama = new OllamaService(ollamaUrl, embedModel, completionModel);
        _vectorStore = new FaissVectorStore(faissUrl);
    }
    
    public async Task AddDocumentAsync(string content)
    {
        var embedding = await _ollama.GetEmbeddingAsync(content);
        await _vectorStore.AddDocumentAsync(content, embedding);
    }
    
    public async Task AddDocumentsBatchAsync(List<string> documents)
    {
        var documentsWithEmbeddings = new List<(string content, float[] embedding)>();
        
        foreach (var content in documents)
        {
            var embedding = await _ollama.GetEmbeddingAsync(content);
            documentsWithEmbeddings.Add((content, embedding));
        }
        
        await _vectorStore.AddDocumentsBatchAsync(documentsWithEmbeddings);
    }
    
    /// <summary>
    /// Load existing vectors from cache and restore FAISS index
    /// </summary>
    public async Task<bool> LoadExistingDataAsync()
    {
        return await _vectorStore.LoadFromCacheAsync();
    }
    
    /// <summary>
    /// Save current state to disk
    /// </summary>
    public async Task SaveAsync()
    {
        await _vectorStore.SaveAsync();
    }
    
    /// <summary>
    /// Clear all data and caches
    /// </summary>
    public async Task ClearAllDataAsync()
    {
        await _vectorStore.ClearAsync();
    }
    
    /// <summary>
    /// Get information about cached data
    /// </summary>
    public (int documentCount, bool hasCache, DateTime? cacheCreated) GetCacheInfo()
    {
        return (_vectorStore.DocumentCount, _vectorStore.HasCache, _vectorStore.CacheCreatedAt);
    }
    
    public async Task<string> AskAsync(string question)
    {
        // Get embedding for the question
        var questionEmbedding = await _ollama.GetEmbeddingAsync(question);
        
        // Find similar documents
        var relevantDocs = await _vectorStore.SearchSimilarAsync(questionEmbedding);
        
        if (relevantDocs.Count == 1 && (relevantDocs[0] == "No relevant documents found." || relevantDocs[0] == "Error retrieving documents."))
        {
            return "I don't have enough information to answer that question.";
        }

        Console.WriteLine($"Asking LLM to confirm\n\n");

        // Create context from relevant documents
        var context = string.Join("\n\n", relevantDocs);
        
        // Generate response
        var prompt = $"""
        You are a strict AI assistant. You MUST answer ONLY using the provided context.
        
        {context}
        
        Question: {question}
        
        Answer:
        """;
        
        return await _ollama.GetCompletionAsync(prompt);
    }
} 