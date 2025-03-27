using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _245_Ollama_Document_Processing_Agent
{
    class Program
    {
        private static readonly string OllamaEndpoint = "http://localhost:11434/api/chat";
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string SampleTextsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SampleTexts");
        private static readonly string OutputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Analysis");
        //private static readonly string ModelName = "phi3:mini";
        private static readonly string ModelName = "tinyllama";

        static async Task Main()
        {
            Console.WriteLine("Document Processing Agent");
            Console.WriteLine("============================================");
            Console.WriteLine($"Using Ollama with model: {ModelName}");
            Console.WriteLine();

            // Ensure Ollama is running and the model is available
            if (!await TestOllamaConnection())
            {
                Console.WriteLine($"\nPlease make sure Ollama is running and the {ModelName} model is installed.");
                Console.WriteLine($"Run this command to install the model: ollama pull {ModelName}");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            // Ensure directories exist
            EnsureDirectoriesExist();

            // Get all text files in the samples directory
            var textFiles = Directory.GetFiles(SampleTextsDirectory, "*.txt");
            
            if (textFiles.Length == 0)
            {
                Console.WriteLine($"No text files found in {SampleTextsDirectory}. Please add some text files and run again.");
                return;
            }

            Console.WriteLine($"Found {textFiles.Length} document(s):");
            for (int i = 0; i < textFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(textFiles[i])}");
            }

            Console.WriteLine();
            Console.WriteLine("Select an operation:");
            Console.WriteLine("1. Summarize a single document");
            Console.WriteLine("2. Compare multiple documents");
            Console.WriteLine("3. Exit");

            bool exitRequested = false;
            while (!exitRequested)
            {
                Console.Write("\nSelect an option (1-3): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await SummarizeDocument(textFiles);
                        break;
                    case "2":
                        await CompareDocuments(textFiles);
                        break;
                    case "3":
                        exitRequested = true;
                        Console.WriteLine("\nExiting application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 3.");
                        break;
                }
            }
        }

        private static void EnsureDirectoriesExist()
        {
            if (!Directory.Exists(SampleTextsDirectory))
            {
                Console.WriteLine($"SampleTexts directory not found at: {SampleTextsDirectory}");
                Console.WriteLine("Please make sure the SampleTexts directory exists with your text files.");
            }

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
                Console.WriteLine($"Created directory for analysis output at: {OutputDirectory}");
            }
        }
        
        private static async Task<bool> TestOllamaConnection()
        {
            Console.WriteLine($"Testing connection to Ollama with model: {ModelName}");
            
            try
            {
                var requestBody = new
                {
                    model = ModelName,
                    messages = new[] { new { role = "user", content = "Test message. Reply with OK." } },
                    stream = false
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(OllamaEndpoint, content);
                response.EnsureSuccessStatusCode();

                Console.WriteLine("Successfully connected to Ollama!");
                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error connecting to Ollama: {ex.Message}");
                Console.WriteLine("\nPossible causes:");
                Console.WriteLine("1. Ollama is not running. Start it with 'ollama serve'");
                Console.WriteLine($"2. The model '{ModelName}' is not installed. Pull it with 'ollama pull {ModelName}'");
                Console.WriteLine("3. Ollama is running on a different endpoint than http://localhost:11434");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return false;
            }
        }

        private static async Task SummarizeDocument(string[] textFiles)
        {
            Console.WriteLine("\nDocument Summarization");
            Console.Write("Enter the number of the document to summarize: ");
            
            if (!int.TryParse(Console.ReadLine(), out int fileIndex) || fileIndex < 1 || fileIndex > textFiles.Length)
            {
                Console.WriteLine("Invalid selection. Please try again.");
                return;
            }

            string selectedFile = textFiles[fileIndex - 1];
            Console.WriteLine($"Processing: {Path.GetFileName(selectedFile)}");

            string documentText = File.ReadAllText(selectedFile);
            Console.WriteLine($"Loaded document with {documentText.Length} characters. Generating summary...");

            var summaryFormat = new
            {
                type = "object",
                properties = new
                {
                    title = new { type = "string" },
                    summary = new { type = "string" },
                    keyPoints = new { type = "array", items = new { type = "string" } },
                    topics = new { type = "array", items = new { type = "string" } }
                },
                required = new[] { "title", "summary", "keyPoints" }
            };

            await ProcessStructuredPrompt(
                $"Summarize the following document in a concise way: {documentText}", 
                summaryFormat, 
                $"summary_{Path.GetFileNameWithoutExtension(selectedFile)}"
            );
        }

        private static async Task CompareDocuments(string[] textFiles)
        {
            if (textFiles.Length < 2)
            {
                Console.WriteLine("You need at least 2 documents to compare. Please add more files.");
                return;
            }

            Console.WriteLine("\nDocument Comparison");
            Console.WriteLine("Select documents to compare (comma-separated list of numbers):");
            for (int i = 0; i < textFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(textFiles[i])}");
            }

            Console.Write("\nEnter selection (e.g., 1,2): ");
            string selectionInput = Console.ReadLine();
            
            string[] selections = selectionInput.Split(',');
            if (selections.Length < 2)
            {
                Console.WriteLine("You must select at least 2 files for comparison.");
                return;
            }

            List<int> selectedIndices = new List<int>();
            foreach (string selection in selections)
            {
                if (int.TryParse(selection.Trim(), out int index) && index >= 1 && index <= textFiles.Length)
                {
                    selectedIndices.Add(index);
                }
            }

            if (selectedIndices.Count < 2)
            {
                Console.WriteLine("You need to select at least 2 valid files.");
                return;
            }

            List<string> selectedTexts = new List<string>();
            List<string> fileNames = new List<string>();

            foreach (int index in selectedIndices)
            {
                string fileName = Path.GetFileName(textFiles[index - 1]);
                fileNames.Add(fileName);
                
                string documentText = File.ReadAllText(textFiles[index - 1]);
                selectedTexts.Add(documentText);
                
                Console.WriteLine($"Loaded: {fileName}");
            }

            var comparisonFormat = new
            {
                type = "object",
                properties = new
                {
                    documents = new
                    {
                        type = "array",
                        items = new
                        {
                            type = "object",
                            properties = new
                            {
                                fileName = new { type = "string" },
                                mainTopic = new { type = "string" }
                            }
                        }
                    },
                    commonalities = new { type = "array", items = new { type = "string" } },
                    differences = new { type = "array", items = new { type = "string" } }
                },
                required = new[] { "commonalities", "differences" }
            };

            string prompt = "Compare the following documents and identify key similarities and differences:\n\n";
            for (int i = 0; i < fileNames.Count; i++)
            {
                prompt += $"Document {i + 1}: {fileNames[i]}\n{selectedTexts[i]}\n\n";
            }

            // Create a descriptive output filename
            string outputBaseName = "comparison_" + string.Join("_vs_", 
                selectedIndices.Select(idx => Path.GetFileNameWithoutExtension(textFiles[idx - 1]))
                             .Take(2)); // Only use first two filenames in the output name
            
            await ProcessStructuredPrompt(prompt, comparisonFormat, outputBaseName);
        }

        private static async Task ProcessStructuredPrompt(string prompt, object format, string outputBaseName = null)
        {
            Console.WriteLine("\nSending request to AI model...");

            try
            {
                var requestBody = new
                {
                    model = ModelName,
                    messages = new[] { new { role = "user", content = prompt } },
                    stream = false,
                    format = format
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(OllamaEndpoint, content);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var parsedResponse = JsonSerializer.Deserialize<OllamaResponse>(jsonResponse);

                if (parsedResponse?.Message?.Content != null)
                {
                    try
                    {
                        string cleanedJson = CleanJsonString(parsedResponse.Message.Content);
                        var structuredData = JsonSerializer.Deserialize<JsonElement>(cleanedJson);

                        // Generate output filename
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string outputFileName = string.IsNullOrEmpty(outputBaseName)
                            ? $"analysis_{timestamp}.json"
                            : $"{outputBaseName}_{timestamp}.json";
                        
                        string outputPath = Path.Combine(OutputDirectory, outputFileName);
                        
                        // Save the JSON to a file
                        File.WriteAllText(outputPath, JsonSerializer.Serialize(structuredData, new JsonSerializerOptions { WriteIndented = true }));

                        Console.WriteLine($"\nAnalysis complete! Results saved to: {outputPath}");
                        Console.WriteLine("\nStructured AI Response:");
                        Console.WriteLine(JsonSerializer.Serialize(structuredData, new JsonSerializerOptions { WriteIndented = true }));
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"\nCould not parse structured response: {ex.Message}");
                        Console.WriteLine("Raw message:");
                        Console.WriteLine(parsedResponse.Message.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        private static string CleanJsonString(string rawContent)
        {
            rawContent = rawContent.Trim();

            if (rawContent.StartsWith("{") && rawContent.EndsWith("}"))
            {
                return rawContent; // already JSON
            }
            else if (rawContent.Contains("{") && rawContent.Contains("}"))
            {
                int startIndex = rawContent.IndexOf("{");
                int endIndex = rawContent.LastIndexOf("}");
                return rawContent.Substring(startIndex, endIndex - startIndex + 1);
            }
            return "{}";
        }
    }

    // Response Model
    public class OllamaResponse
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("message")]
        public OllamaMessage Message { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }

        [JsonPropertyName("done_reason")]
        public string DoneReason { get; set; }
    }

    public class OllamaMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
