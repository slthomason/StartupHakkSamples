using Simple_RAG_NET.Services;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Simple RAG Implementation ===\n");

        // Configure Ollama settings
        string ollamaUrl = "http://servers.askmanyai.net:8889/ollama";
        string embedModel = "snowflake-arctic-embed2:latest";
        string completionModel = "llama3.2:latest";
        
        // Configure FAISS service URL
        string faissUrl = "http://localhost:8001"; // Change to your FAISS service URL
        
        // Create RAG service
        var rag = new RagService(ollamaUrl, embedModel, completionModel, faissUrl);

        // Check for existing data
        Console.WriteLine("Checking for existing data...");
        var cacheInfo = rag.GetCacheInfo();
        
        if (cacheInfo.hasCache)
        {
            Console.WriteLine($"Found existing cache with {cacheInfo.documentCount} documents (created: {cacheInfo.cacheCreated})");
            Console.WriteLine("Loading existing data...");
            
            var loadSuccess = await rag.LoadExistingDataAsync();
            if (loadSuccess)
            {
                Console.WriteLine($"Successfully loaded {cacheInfo.documentCount} documents from cache");
            }
            else
            {
                Console.WriteLine("Failed to load from cache, will start fresh");
            }
        }
        else
        {
            Console.WriteLine("No existing cache found, starting fresh");
        }

        // Add sample documents if we don't have any data
        if (cacheInfo.documentCount == 0)
        {
            var documents = new[]
            {
                // Animals & Biology
                "Cats are small, domesticated mammals. They are known for their independence and hunting skills. They have excellent night vision and can rotate their ears 180 degrees.",
                "Dogs are loyal pets that have been domesticated for thousands of years. They are known for their companionship and come in over 300 different breeds worldwide.",
                "Birds are warm-blooded vertebrates with feathers. They can fly and lay eggs. Some birds like penguins cannot fly but are excellent swimmers.",
                "Fish live in water and breathe through gills. They come in many shapes and sizes, from tiny goldfish to massive whale sharks.",
                "Elephants are the largest land mammals on Earth. They have excellent memories and can live up to 70 years. They use their trunks for breathing, drinking, and grabbing objects.",
                "Dolphins are highly intelligent marine mammals. They use echolocation to navigate and communicate with each other through complex vocalizations.",
                "Bees are crucial pollinators that help plants reproduce. A single bee colony can contain up to 80,000 bees during peak season.",
                
                // Technology & Science
                "Artificial Intelligence (AI) is the simulation of human intelligence in machines. It includes machine learning, natural language processing, and computer vision.",
                "Machine Learning is a subset of AI that enables computers to learn and improve from experience without being explicitly programmed.",
                "Python is a high-level programming language known for its simplicity and readability. It's widely used in data science, web development, and AI.",
                "The Internet is a global network of interconnected computers that allows people to share information and communicate worldwide.",
                "Smartphones are mobile devices that combine computing power with telecommunications. They typically include cameras, GPS, and internet connectivity.",
                "Cloud computing allows users to access computing resources and services over the internet without owning physical hardware.",
                "Renewable energy sources include solar, wind, hydroelectric, and geothermal power. They are sustainable alternatives to fossil fuels.",
                
                // Space & Physics
                "The Sun is a medium-sized star located at the center of our solar system. It provides heat and light to Earth and is about 93 million miles away.",
                "The Moon is Earth's only natural satellite. It orbits Earth every 27.3 days and influences ocean tides through gravitational pull.",
                "Mars is the fourth planet from the Sun and is known as the Red Planet due to iron oxide on its surface. It has two small moons.",
                "Black holes are regions of space where gravity is so strong that nothing, not even light, can escape once it crosses the event horizon.",
                "The speed of light in a vacuum is approximately 299,792,458 meters per second, making it the universal speed limit.",
                
                // Geography & History
                "The Amazon Rainforest is the world's largest tropical rainforest, covering much of the Amazon Basin in South America. It's often called the 'lungs of the Earth'.",
                "Mount Everest is the highest mountain on Earth, standing at 29,032 feet (8,849 meters) above sea level. It's located in the Himalayas.",
                "The Great Wall of China is an ancient fortification system built to protect Chinese states from invasions. It stretches over 13,000 miles.",
                "The Pacific Ocean is the largest and deepest ocean on Earth, covering about one-third of the planet's surface.",
                "Ancient Egypt was a civilization that flourished along the Nile River for over 3,000 years. They built impressive pyramids and developed hieroglyphic writing.",
                
                // Human Body & Health
                "The human brain contains approximately 86 billion neurons and uses about 20% of the body's total energy despite being only 2% of body weight.",
                "The heart is a muscular organ that pumps blood throughout the body. It beats about 100,000 times per day and 2.5 billion times in a lifetime.",
                "Vitamin D is essential for bone health and immune function. It can be obtained from sunlight exposure, certain foods, and supplements.",
                "Exercise provides numerous health benefits including improved cardiovascular health, stronger muscles and bones, and better mental health.",
                
                // Food & Culture
                "Pizza originated in Naples, Italy, in the 18th century. The classic Margherita pizza was created in 1889 to represent the Italian flag colors.",
                "Coffee is one of the world's most popular beverages, originating from Ethiopia. It contains caffeine, which acts as a natural stimulant.",
                "Rice is a staple food for over half of the world's population. It's primarily grown in Asia and comes in thousands of varieties.",
                "Chocolate is made from cacao beans that grow on trees in tropical regions. Dark chocolate contains antioxidants that may benefit heart health.",
                
                // Environment & Climate
                "Climate change refers to long-term changes in global temperatures and weather patterns, primarily caused by human activities since the Industrial Revolution.",
                "Photosynthesis is the process by which plants convert sunlight, carbon dioxide, and water into glucose and oxygen, forming the basis of most food chains.",
                "Recycling helps reduce waste and conserve natural resources. Common recyclable materials include paper, plastic, glass, and metal.",
                "Biodiversity refers to the variety of life on Earth, including different species, ecosystems, and genetic variations within species."
            };

            Console.WriteLine("Adding comprehensive knowledge base...");
            await rag.AddDocumentsBatchAsync(documents.ToList());
        }

        // Final cache info
        var finalCacheInfo = rag.GetCacheInfo();
        Console.WriteLine($"Ready! Knowledge base contains {finalCacheInfo.documentCount} documents.");
        Console.WriteLine("Ask questions about animals, technology, science, geography, health, or any topic!");
        Console.WriteLine("Commands: 'save' to save data, 'clear' to clear all data, 'info' for statistics.\n");

        // Simple Q&A loop
        while (true)
        {
            Console.Write("Question: ");
            var question = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(question) || question.ToLower() == "quit")
                break;

            // Special commands
            if (question.ToLower() == "save")
            {
                Console.WriteLine("Saving data...");
                await rag.SaveAsync();
                Console.WriteLine("Data saved successfully!\n");
                continue;
            }
            
            if (question.ToLower() == "clear")
            {
                Console.WriteLine("Clearing all data...");
                await rag.ClearAllDataAsync();
                Console.WriteLine("All data cleared!\n");
                continue;
            }
            
            if (question.ToLower() == "info")
            {
                var info = rag.GetCacheInfo();
                Console.WriteLine($"Knowledge base info:");
                Console.WriteLine($"  Documents: {info.documentCount}");
                Console.WriteLine($"  Cache exists: {info.hasCache}");
                Console.WriteLine($"  Cache created: {info.cacheCreated}\n");
                continue;
            }

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