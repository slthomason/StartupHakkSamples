# MCP WebSurfer

A Model Context Protocol (MCP) server built using the C# SDK that provides AI-powered tools for web surfing and content extraction.

## Project Structure

- **Program.cs**: Main entry point that configures logging, registers services, and sets up the MCP server
- **WebSurferTool.cs**: Tools for browsing the web, extracting content, and summarizing web pages

## Features

### Web Surfing Tools

- **WebPageContent**: Fetches and returns the HTML content of a web page
- **WebPageText**: Extracts and returns just the text content from a web page
- **WebPageLinks**: Extracts links from a web page, with customizable limit
- **WebPageTitle**: Gets the title of a web page
- **WebSearch**: Performs a simple search and returns the top results
- **SummarizeWebPage**: Summarizes the content of a web page in a few sentences

## Getting Started

1. Clone this repository
2. Build the project with `dotnet build`
3. Run the server with `dotnet run`

## Configuration

The MCP server uses standard input/output for communication. The server is configured with:
- Debug-level logging
- Console output for all log levels
- HttpClient with proper timeout settings

## Using with Claude

You can use these tools with Claude by:

1. Setting up Claude to connect to your MCP server
2. Asking questions that use the available tools, such as:
   - "Fetch the title of https://microsoft.com"
   - "Extract all links from https://github.com"
   - "Summarize the content of https://en.wikipedia.org/wiki/Artificial_intelligence"
   - "Search for information about quantum computing"

## VS Code Integration

To use this server with VS Code and GitHub Copilot:

1. Configure `.vscode/mcp.json`:
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

2. Select the MCPServer.CSharp server in GitHub Copilot

## Error Handling

The WebSurfer tools include comprehensive error handling:
- Invalid URLs are automatically normalized
- Network errors are caught and reported
- Content extraction failures are handled gracefully
- All operations have timeout protection

## Dependencies

- .NET 8.0
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.Http
- Microsoft.Extensions.Logging
- ModelContextProtocol 