# MCPServer.CSharp

A Model Context Protocol (MCP) server built using the C# SDK that provides AI-powered tools for web surfing, content safety analysis, and campaign analytics.

## Project Structure

- **Program.cs**: Main entry point that configures logging, registers services, and sets up the MCP server
- **WebSurferTool.cs**: Tools for browsing the web, extracting content, and summarizing web pages
- **CleanRouterService.cs**: Service for analyzing website safety and categorization
- **CleanRouterCrawlerTool.cs**: Web crawler tool for batch analysis of websites
- **ViciNotebookService.cs**: Backend service for notebook management
- **ViciNotebookTool.cs**: Tools for notebook creation and management
- **ViciCampaignReportTool.cs**: Tools for campaign analytics and reporting

## Features

### Web Surfing Tools

- **WebPageContent**: Fetches and returns the HTML content of a web page
- **WebPageText**: Extracts and returns just the text content from a web page
- **WebPageLinks**: Extracts links from a web page, with customizable limit
- **WebPageTitle**: Gets the title of a web page
- **WebSearch**: Performs a simple search and returns the top results
- **SummarizeWebPage**: Summarizes the content of a web page in a few sentences

### Web Safety Tools

- **ListAnalyzedSites**: Lists websites that have been analyzed, optionally filtered by category
- **AnalyzePageSafety**: Analyzes a webpage for safety and content categorization
- **StartCatalogCrawler**: Starts cataloging websites from seed URLs
- **GetCatalogStatus**: Gets the current status of the internet cataloging process
- **ImportUrlList**: Imports a list of URLs to analyze in batch mode
- **GenerateCatalogReport**: Generates a summary report of the catalog

### Campaign Reporting Tools

- **ListCampaigns**: Lists all available campaign IDs with names
- **GenerateCampaignReport**: Generates a JSON analytics report for a campaign

### Notebook Tools

- **ListNotebooks**: Lists all analysis notebooks
- **CreateNotebook**: Creates a new analysis notebook for a campaign
- **GetNotebook**: Gets a specific analysis notebook with all its cells
- **AddMarkdownCell**: Adds a markdown cell to a notebook
- **ExecuteCell**: Executes a cell and returns its results

## Getting Started

1. Clone this repository
2. Build the project with `dotnet build`
3. Run the server with `dotnet run`

## Configuration

By default, the MCP server uses standard input/output for communication. You can configure it to use Server-Sent Events (SSE) for HTTP-based communication by modifying Program.cs.

## Using with Claude

You can use these tools with Claude by:

1. Setting up Claude to connect to your MCP server
2. Asking questions that use the available tools, such as:
   - "Fetch the title of https://microsoft.com"
   - "Extract all links from https://github.com"
   - "Summarize the content of https://en.wikipedia.org/wiki/Artificial_intelligence"
   - "Analyze the safety of https://example.com"
   - "Generate a report for campaign CAMP-001"

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