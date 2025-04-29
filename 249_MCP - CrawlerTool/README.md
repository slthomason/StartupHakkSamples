# MCPServer.CSharp

A Model Context Protocol (MCP) server built using the C# SDK that provides AI-powered tools for web crawling, content analysis, and website categorization.

## Project Structure

- **Program.cs**: Main entry point that configures logging, registers services, and sets up the MCP server
- **CrawlerService.cs**: Core service for analyzing website safety, content categorization, and maintaining the website catalog
- **CrawlerTool.cs**: MCP tools for web crawling, batch analysis, and catalog management
- **appsettings.json**: Configuration file for application settings
- **MCPServer.CSharp.csproj**: Project configuration and dependencies

## Features

### Web Crawling and Analysis

- **StartCatalogCrawler**: Initiates web crawling from seed URLs with configurable depth and link limits
- **StopCatalogCrawler**: Stops the active crawling process
- **GetCatalogStatus**: Retrieves current status of the crawling process
- **ImportUrlList**: Batch imports URLs for analysis
- **ExportCatalog**: Exports the website catalog to a JSON file
- **GenerateCatalogReport**: Creates detailed reports of the catalog contents

### Website Analysis

- **AnalyzeWebsiteAsync**: Analyzes website content, categorizes it, and checks safety
- **GetWebsiteDetailsAsync**: Retrieves detailed information about a specific website
- **GetAvailableCategories**: Lists all available website categories
- **GetMostCommonKeywords**: Identifies frequently occurring keywords in the catalog

### Catalog Management

- **GetCatalogSize**: Returns the total number of analyzed websites
- **GetCatalogEntriesAsync**: Retrieves websites by category
- **SaveCatalogToFileAsync**: Exports the catalog to a file
- **ImportUrlListAsync**: Imports a list of URLs for analysis

## Getting Started

1. **Prerequisites**
   - .NET 8.0 SDK
   - Visual Studio 2022 or VS Code with C# extension

2. **Installation**
   ```bash
   git clone [repository-url]
   cd MCPServer.CSharp
   dotnet restore
   ```

3. **Building**
   ```bash
   dotnet build
   ```

4. **Running**
   ```bash
   dotnet run
   ```

## Configuration

The application can be configured through `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

## Using with Claude

The MCP server can be used with Claude by:

1. Setting up Claude to connect to your MCP server
2. Using the available tools through natural language commands, such as:
   - "Start crawling from these URLs: example.com, test.com"
   - "Get the current status of the catalog"
   - "Generate a report of the analyzed websites"
   - "Export the catalog to a file"

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

## Container Support

The project includes container support for:
- Linux x64
- Linux ARM64

To build the container:
```bash
dotnet publish -c Release -r linux-x64
```

## License

[Add your license information here] 