using OpenAI.Chat;

ChatClient client = new(model: "gpt-4o", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
ChatMessage messages =
    new UserChatMessage("What's the weather like today?");

ChatCompletion completion = client.CompleteChat(messages);

Console.WriteLine($"[ASSISTANT]: {completion}");