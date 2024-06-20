using Aspire.Hosting.Azure;

public class OpenAIService
{
    private readonly OpenAIClient _openAiClient;

    public OpenAIService(OpenAIClient openAiClient)
    {
        _openAiClient = openAiClient;
    }

    public string GetOpenAIResponse(string input)
    {
        // Example usage of OpenAIClient
        var response = _openAiClient.Completions.CreateCompletionAsync(
            new CompletionRequest
            {
                Model = "text-davinci-002",
                Prompt = input,
                MaxTokens = 50
            }).Result;

        return response?.Choices[0]?.Text ?? "No response";
    }
}
