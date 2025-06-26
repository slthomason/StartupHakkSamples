using Simple_RAG_NET.Services;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Simple RAG Implementation ===\n");

        // Configure Ollama settings
        string ollamaUrl = "http://localhost:11434"; // Default Ollama URL, change if needed
        string model = "phi3"; // Default model, change to any model available in your Ollama instance
        
        // Create RAG service
        var rag = new RagService(ollamaUrl, model);

        // Add some sample documents
        var documents = new[]
        {
            "Cats are small, domesticated mammals. They are known for their independence and hunting skills.",
            "Dogs are loyal pets that have been domesticated for thousands of years. They are known for their companionship.",
            "Birds are warm-blooded vertebrates with feathers. They can fly and lay eggs.",
            "Fish live in water and breathe through gills. They come in many shapes and sizes."
        };

        Console.WriteLine("Adding documents...");
        foreach (var doc in documents)
        {
            await rag.AddDocumentAsync(doc);
        }

        Console.WriteLine("Ready! Ask questions about animals.\n");

        // Simple Q&A loop
        while (true)
        {
            Console.Write("Question: ");
            var question = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(question) || question.ToLower() == "quit")
                break;

            try
            {
                var answer = await rag.AskAsync(question);
                Console.WriteLine($"Answer: {answer}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }
        }

        Console.WriteLine("Goodbye!");
    }
}