namespace Simple_RAG_NET.Services;

public class VectorStore
{
    private readonly List<(string content, float[] embedding)> _documents = new();
    
    public void AddDocument(string content, float[] embedding)
    {
        _documents.Add((content, embedding));
        Console.WriteLine($"Added document: {content.Substring(0, Math.Min(30, content.Length))}...");
    }
    
    public List<string> SearchSimilar(float[] queryEmbedding, int topK = 3)
    {
        if (!_documents.Any())
        {
            Console.WriteLine("No documents in vector store!");
            return new List<string> { "No relevant documents found." };
        }
        
        var similarities = _documents
            .Select(doc => (doc.content, similarity: CosineSimilarity(queryEmbedding, doc.embedding)))
            .OrderByDescending(x => x.similarity)
            .Take(topK)
            .ToList();
            
        Console.WriteLine($"Found {similarities.Count} documents with similarities:");
        foreach (var (content, similarity) in similarities)
        {
            Console.WriteLine($"  - {similarity:F3}: {content.Substring(0, Math.Min(50, content.Length))}...");
        }
        
        var results = similarities
            .Where(x => x.similarity > 0.15)
            .Select(x => x.content)
            .ToList();
            
        Console.WriteLine($"Returning {results.Count} documents above threshold 0.15");
        
        if (!results.Any() && similarities.Any())
        {
            Console.WriteLine("No documents above threshold, returning best match");
            return new List<string> { similarities.First().content };
        }
        
        return results.Any() ? results : new List<string> { "No relevant documents found." };
    }
    
    private static float CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length)
            throw new ArgumentException("Vectors must have same length");
            
        float dotProduct = 0, normA = 0, normB = 0;
        
        for (int i = 0; i < a.Length; i++)
        {
            dotProduct += a[i] * b[i];
            normA += a[i] * a[i];
            normB += b[i] * b[i];
        }
        
        return dotProduct / (float)(Math.Sqrt(normA) * Math.Sqrt(normB));
    }
} 