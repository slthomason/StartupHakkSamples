# MCPServer.CSharp

A Model Context Protocol (MCP) server built using the C# SDK. It provides several tools that can be consumed by MCP clients like VS Code with GitHub Copilot.

## Features

- **Echo Tools**: Simple echo and reverse echo functionality
- **Monkey Tools**: Fetch monkey data from a remote API
- **LLM Tools**: Interact with X.AI (Grok) and local Ollama for AI capabilities
- **File System Tools**: List directories, read/write files, and search for content

## How to Run

### Local Development

1. Clone this repository
2. Make sure you have Ollama running locally (`http://localhost:11434`)
3. Configure `appsettings.json` with your API keys and endpoints
4. Run the server with `dotnet run`

### VS Code Integration

1. Configure VS Code by adding this entry to your `.vscode/mcp.json` file:

```json
{
    "inputs": [],
    "servers": {
        "MCPServer.CSharp": {
            "type": "stdio",
            "command": "dotnet",
            "args": [
                "run",
                "--project",
                "${workspaceFolder}/MCPServer.CSharp.csproj"
            ]
        }
    }
}
```

2. Open VS Code and enable GitHub Copilot (Agent mode)
3. Select the MCPServer.CSharp server from the dropdown
4. Start using the tools by typing prompts

## Configuration

The server uses `appsettings.json` to configure API keys and endpoints:

```json
{
  "LlmServiceConfig": {
    "ApiKeys": {
      "Grok": "your-xai-api-key"
    },
    "Endpoints": {
      "Ollama": "http://localhost:11434"
    }
  }
}
```

## Available Tools

### Echo Tools
- `Echo`: Returns the input message
- `ReverseEcho`: Returns the input message reversed

### Monkey Tools
- `GetMonkeys`: Returns a list of monkeys
- `GetMonkey`: Returns details about a specific monkey by name

### LLM Tools
- `AskGrok`: Sends a prompt to X.AI's Grok-2 model (API key optional if configured)
- `AskOllama`: Sends a prompt to local Ollama using the phi3:mini model
- `ListOllamaModels`: Lists available models in your local Ollama server

### File System Tools
- `ListDirectory`: Lists files and directories in a specified path
- `ReadFile`: Reads the content of a text file
- `WriteFile`: Writes or appends content to a text file
- `SearchFiles`: Searches for files containing specific text

## Example Prompts

- "Echo hello world"
- "Get information about the Baboon monkey"
- "Ask Grok to write a poem about coding"
- "Ask Ollama to explain how to use async/await in C#"
- "Ask Ollama with the model phi3:mini 'Explain how transformers work in machine learning'"
- "List all available models in my local Ollama server"
- "List all files in my Documents directory"
- "Search for files containing 'MCP' in the current directory"

## Prerequisites

1. Install Ollama from https://ollama.com/
2. Run Ollama locally
3. Pull the phi3:mini model:
   ```
   ollama pull phi3:mini
   ```

## Docker Deployment

Build and publish a container image:

```
dotnet publish /t:PublishContainer
```

Push to a container registry:

```
dotnet publish /t:PublishContainer -p ContainerRegistry=docker.io
```

Configure a client to use the Docker container:

```json
{
    "inputs": [],
    "servers": {
        "mcpserver-csharp": {
            "command": "docker",
            "args": [
                "run",
                "-i",
                "--rm",
                "mcpserver-csharp"
            ],
            "env": {}
        }
    }
}
```

## Server-Sent Events (SSE)

For a more scalable approach, you can deploy this server using SSE transport:

1. Add ASP.NET Core to your project
2. Configure the server to use SSE transport
3. Deploy to a hosting service like Azure App Service or Container Apps

See the [MCP C# SDK samples](https://github.com/modelcontextprotocol/csharp-sdk/tree/main/samples) for examples of SSE implementation. 