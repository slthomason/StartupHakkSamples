# MCPServer.CSharp

A Model Context Protocol (MCP) server built using the C# SDK that provides tools for campaign analytics and notebook-based data analysis.

## Project Structure

- **Program.cs**: Main entry point that configures logging, registers services, and sets up the MCP server
- **NotebookService.cs**: Core service managing campaigns, notebooks, and analysis cells
- **NotebookTool.cs**: Tools for notebook creation and management
- **CampaignReportTool.cs**: Tools for campaign analytics and reporting

## Features

### Campaign Reporting Tools

- **ListCampaigns**: Lists all available campaign IDs with basic information (ID, name, description, start date, call count)
- **GenerateCampaignReport**: Generates a detailed JSON analytics report for a campaign, including:
  - Campaign metadata (ID, name, description)
  - Call statistics (total calls, calls per day)
  - Duration analysis (average and median call duration)
  - Call status distribution
  - Disposition summary
  - Agent performance metrics

### Notebook Tools

- **ListNotebooks**: Lists all analysis notebooks with metadata
- **CreateNotebook**: Creates a new analysis notebook for a campaign
- **GetNotebook**: Retrieves a specific notebook with all its cells
- **AddMarkdownCell**: Adds a markdown cell for documentation
- **AddDataQueryCell**: Adds a cell for SQL-like queries against campaign data
- **AddChartCell**: Adds a cell for data visualization
- **AddInsightCell**: Adds an AI-powered analysis cell
- **ExecuteCell**: Executes a cell and returns its results
- **GetCampaignInsights**: Generates AI-powered insights for campaign analysis

## Data Models

### Campaign Data
- **Campaign**: Contains campaign metadata and a collection of calls
- **CampaignCall**: Individual call records with:
  - Status (COMPLETED, NO_ANSWER, BUSY, VOICEMAIL, REJECTED)
  - Disposition codes (varies by campaign type)
  - Agent information
  - Duration and timestamp
  - Contact details

### Notebook System
- **AnalysisNotebook**: Container for analysis cells
- **NotebookCell**: Individual analysis units with types:
  - Markdown: Documentation and notes
  - Data: SQL-like queries
  - Chart: Data visualizations
  - Insight: AI-powered analysis

## Getting Started

1. Clone this repository
2. Build the project with `dotnet build`
3. Run the server with `dotnet run`

## Example Usage

```csharp
// List available campaigns
ListCampaigns()

// Generate a report for a specific campaign
GenerateCampaignReport("CAMP-001")

// Create and populate a notebook
CreateNotebook("Q2 Analysis", "CAMP-001")
AddMarkdownCell(notebookId, "# Campaign Analysis\nAnalyzing Q2 performance...")
AddDataQueryCell(notebookId, "SELECT * FROM Calls WHERE Status = 'COMPLETED'")
AddChartCell(notebookId, "BAR CHART: Call Dispositions")
AddInsightCell(notebookId, "ANALYZE: What are the trends in customer satisfaction?")
```

## Configuration

The MCP server uses standard input/output for communication by default. The server is configured with:
- Debug-level logging
- Console logging with trace-level error reporting
- Singleton service registration for NotebookService

## VS Code Integration

To use this server with VS Code:

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

2. Select the MCPServer.CSharp server in your development environment 