using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

class Program
{
    private static readonly string OllamaEndpoint = "http://localhost:11434/api/chat";
    private static readonly HttpClient httpClient = new HttpClient();

    static async Task Main()
    {
        Console.WriteLine("Ollama Structured Chat Client");
        Console.WriteLine("============================================");
        while (true)
        {
            Console.WriteLine("1. Country Information - Get details about a country (capital, population, languages, etc.)");
            Console.WriteLine("2. Recipe Format - Get a structured recipe with ingredients, steps, and preparation time.");
            Console.WriteLine("3. Product Catalog - View product categories with specifications and availability.");
            Console.WriteLine("4. Weather Forecast - Get current conditions and a 7-day forecast for a location.");
            Console.WriteLine("5. Exit - Close the application.");
            Console.WriteLine("===================================");

            Console.Write("\nSelect an option (1-5): ");
            string choice = Console.ReadLine();

            if (choice == "5")
            {
                Console.WriteLine("\nExiting application. Goodbye!");
                break;
            }

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nFetching Country Information...");
                    await RunCountryDemo();
                    break;
                case "2":
                    Console.WriteLine("\nFetching Recipe Details...");
                    await RunRecipeDemo();
                    break;
                case "3":
                    Console.WriteLine("\nFetching Product Catalog...");
                    await RunProductDemo();
                    break;
                case "4":
                    Console.WriteLine("\nFetching Weather Forecast...");
                    await RunWeatherDemo();
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 5.");
                    break;
            }
        }

    }

    private static async Task RunCountryDemo()
    {
        var format = new
        {
            type = "object",
            properties = new
            {
                name = new { type = "string" },
                capital = new { type = "string" },
                population = new { type = "number" },
                languages = new { type = "array", items = new { type = "string" } },
                landmarks = new { type = "array", items = new { type = "string" } }
            },
            required = new[] { "name", "capital", "population", "languages", "landmarks" }
        };

        Console.WriteLine("\nCountry Information");

        while (true)
        {
            Console.Write("\nEnter country name (or 'back' to return): ");
            string userPrompt = Console.ReadLine();
            if (string.IsNullOrEmpty(userPrompt) || userPrompt.ToLower() == "back")
                break;

            await ProcessStructuredPrompt(userPrompt, format);
        }
    }

    private static async Task RunRecipeDemo()
    {
        var format = new
        {
            type = "object",
            properties = new
            {
                recipeName = new { type = "string" },
                preparationTime = new { type = "number" },
                difficulty = new { type = "string", @enum = new[] { "Easy", "Medium", "Hard" } },
                ingredients = new
                {
                    type = "array",
                    items = new
                    {
                        type = "object",
                        properties = new
                        {
                            item = new { type = "string" },
                            amount = new { type = "string" }
                        }
                    }
                },
                steps = new
                {
                    type = "array",
                    items = new { type = "string" }
                }
            },
            required = new[] { "recipeName", "preparationTime", "difficulty", "ingredients", "steps" }
        };

        Console.WriteLine("\nRecipe Format");

        while (true)
        {
            Console.Write("\nEnter recipe name (or 'back' to return): ");
            string userPrompt = Console.ReadLine();
            if (string.IsNullOrEmpty(userPrompt) || userPrompt.ToLower() == "back")
                break;

            await ProcessStructuredPrompt(userPrompt, format);
        }
    }

    private static async Task RunProductDemo()
    {
        var format = new
        {
            type = "object",
            properties = new
            {
                category = new { type = "string" },
                products = new
                {
                    type = "array",
                    items = new
                    {
                        type = "object",
                        properties = new
                        {
                            id = new { type = "string" },
                            name = new { type = "string" },
                            price = new { type = "number" },
                            inStock = new { type = "boolean" },
                            specifications = new
                            {
                                type = "object",
                                additionalProperties = true
                            }
                        }
                    }
                }
            },
            required = new[] { "category", "products" }
        };

        Console.WriteLine("\nProduct Catalog");

        while (true)
        {
            Console.Write("\nEnter product category (or 'back' to return): ");
            string userPrompt = Console.ReadLine();
            if (string.IsNullOrEmpty(userPrompt) || userPrompt.ToLower() == "back")
                break;

            await ProcessStructuredPrompt(userPrompt, format);
        }
    }

    private static async Task RunWeatherDemo()
    {
        var format = new
        {
            type = "object",
            properties = new
            {
                location = new { type = "string" },
                currentConditions = new
                {
                    type = "object",
                    properties = new
                    {
                        temperature = new { type = "number" },
                        humidity = new { type = "number" },
                        windSpeed = new { type = "number" },
                        description = new { type = "string" }
                    }
                },
                forecast = new
                {
                    type = "array",
                    items = new
                    {
                        type = "object",
                        properties = new
                        {
                            day = new { type = "string" },
                            highTemp = new { type = "number" },
                            lowTemp = new { type = "number" },
                            conditions = new { type = "string" }
                        }
                    }
                }
            },
            required = new[] { "location", "currentConditions", "forecast" }
        };

        Console.WriteLine("\nWeather Forecast");

        while (true)
        {
            Console.Write("\nEnter city name (or 'back' to return): ");
            string userPrompt = Console.ReadLine();
            if (string.IsNullOrEmpty(userPrompt) || userPrompt.ToLower() == "back")
                break;

            await ProcessStructuredPrompt(userPrompt, format);
        }
    }


    private static async Task ProcessStructuredPrompt(string userPrompt, object format)
    {
        Console.WriteLine("\nSending request...");

        try
        {
            var requestBody = new
            {
                model = "tinyllama:latest",
                messages = new[] { new { role = "user", content = $"Tell me about {userPrompt}" } },
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

                    Console.WriteLine("\nStructured AI Response:");
                    Console.WriteLine(JsonSerializer.Serialize(structuredData, new JsonSerializerOptions { WriteIndented = true }));
                }
                catch (JsonException)
                {
                    Console.WriteLine("\nCould not parse structured response. Raw message:");
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
