namespace Simple_RAG_NET.Services;

public class RagService
{
    private readonly OllamaService _ollama;
    private readonly VectorStore _vectorStore;
    
    public RagService(string ollamaUrl, string model)
    {
        _ollama = new OllamaService(ollamaUrl, model);
        _vectorStore = new VectorStore();
    }
    
    public async Task AddDocumentAsync(string content)
    {
        var embedding = await _ollama.GetEmbeddingAsync(content);
        _vectorStore.AddDocument(content, embedding);
    }
    
    public async Task<string> AskAsync(string question)
    {
        // Get embedding for the question
        var questionEmbedding = await _ollama.GetEmbeddingAsync(question);
        
        // Find similar documents
        var relevantDocs = _vectorStore.SearchSimilar(questionEmbedding);
        
        if (relevantDocs.Count == 1 && relevantDocs[0] == "No relevant documents found.")
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