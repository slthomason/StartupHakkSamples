using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace MCPServer.CSharp
{
    [McpServerToolType]
    public static class ViciNotebookTool
    {
        [McpServerTool, Description("Lists all analysis notebooks.")]
        public static string ListNotebooks(
            ViciNotebookService notebookService)
        {
            try
            {
                var notebooks = notebookService.GetNotebooks();
                
                var summaries = notebooks.Select(n => new
                {
                    n.Id,
                    n.Title,
                    n.CampaignId,
                    CellCount = n.Cells.Count,
                    n.CreatedAt,
                    n.UpdatedAt
                });
                
                return JsonSerializer.Serialize(summaries, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error listing notebooks: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Creates a new analysis notebook for a campaign.")]
        public static string CreateNotebook(
            ViciNotebookService notebookService,
            [Description("Title of the new notebook")] string title,
            [Description("ID of the campaign to analyze")] string campaignId)
        {
            try
            {
                var notebook = notebookService.CreateNotebook(title, campaignId);
                return JsonSerializer.Serialize(notebook, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error creating notebook: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Gets a specific analysis notebook with all its cells.")]
        public static string GetNotebook(
            ViciNotebookService notebookService,
            [Description("ID of the notebook to retrieve")] string notebookId)
        {
            try
            {
                var notebook = notebookService.GetNotebook(notebookId);
                if (notebook == null)
                    return $"Notebook with ID {notebookId} not found";
                
                return JsonSerializer.Serialize(notebook, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error retrieving notebook: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Adds a markdown cell to a notebook.")]
        public static string AddMarkdownCell(
            ViciNotebookService notebookService,
            [Description("ID of the notebook to add to")] string notebookId,
            [Description("Markdown content to add")] string content)
        {
            try
            {
                var cell = notebookService.AddCell(notebookId, NotebookCellType.Markdown, content);
                if (cell == null)
                    return $"Failed to add cell - notebook {notebookId} not found";
                
                return JsonSerializer.Serialize(cell, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error adding markdown cell: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Adds a data query cell to a notebook.")]
        public static string AddDataQueryCell(
            ViciNotebookService notebookService,
            [Description("ID of the notebook to add to")] string notebookId,
            [Description("SQL-like query for campaign data")] string query)
        {
            try
            {
                var cell = notebookService.AddCell(notebookId, NotebookCellType.Data, query);
                if (cell == null)
                    return $"Failed to add cell - notebook {notebookId} not found";
                
                return JsonSerializer.Serialize(cell, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error adding data query cell: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Adds a chart visualization cell to a notebook.")]
        public static string AddChartCell(
            ViciNotebookService notebookService,
            [Description("ID of the notebook to add to")] string notebookId,
            [Description("Chart specification (e.g., 'BAR CHART: Call Dispositions')")] string chartSpec)
        {
            try
            {
                var cell = notebookService.AddCell(notebookId, NotebookCellType.Chart, chartSpec);
                if (cell == null)
                    return $"Failed to add cell - notebook {notebookId} not found";
                
                return JsonSerializer.Serialize(cell, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error adding chart cell: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Adds an AI insight cell to analyze campaign data.")]
        public static string AddInsightCell(
            ViciNotebookService notebookService,
            [Description("ID of the notebook to add to")] string notebookId,
            [Description("Analysis question (e.g., 'ANALYZE: What are trends in customer satisfaction?')")] string analysisPrompt)
        {
            try
            {
                // Ensure it has the ANALYZE: prefix
                if (!analysisPrompt.StartsWith("ANALYZE:", StringComparison.OrdinalIgnoreCase))
                {
                    analysisPrompt = "ANALYZE: " + analysisPrompt;
                }
                
                var cell = notebookService.AddCell(notebookId, NotebookCellType.Insight, analysisPrompt);
                if (cell == null)
                    return $"Failed to add cell - notebook {notebookId} not found";
                
                return JsonSerializer.Serialize(cell, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error adding insight cell: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Executes a cell and returns its results.")]
        public static async Task<string> ExecuteCell(
            ViciNotebookService notebookService,
            [Description("ID of the notebook containing the cell")] string notebookId,
            [Description("ID of the cell to execute")] string cellId)
        {
            try
            {
                var cell = await notebookService.ExecuteCell(notebookId, cellId);
                if (cell == null)
                    return $"Failed to execute cell - notebook or cell not found";
                
                return JsonSerializer.Serialize(cell, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error executing cell: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Gets AI-powered insights for a campaign.")]
        public static async Task<string> GetCampaignInsights(
            ViciNotebookService notebookService,
            [Description("ID of the campaign to analyze")] string campaignId,
            [Description("Optional specific aspect to analyze (e.g., 'agent performance', 'customer satisfaction')")] string aspect = "")
        {
            try
            {
                // Create a temporary notebook for this analysis
                var notebook = notebookService.CreateNotebook($"Temp Analysis: {aspect}", campaignId);
                
                // Add an insight cell with the appropriate query
                string analysisPrompt = string.IsNullOrEmpty(aspect)
                    ? "ANALYZE: What are the key insights from this campaign?"
                    : $"ANALYZE: What insights can we draw about {aspect} from this campaign?";
                
                var cell = notebookService.AddCell(notebook.Id, NotebookCellType.Insight, analysisPrompt);
                
                // Execute the cell
                var executedCell = await notebookService.ExecuteCell(notebook.Id, cell.Id);
                
                // Return a clean result
                var result = new
                {
                    Campaign = campaignId,
                    AnalysisAspect = string.IsNullOrEmpty(aspect) ? "General" : aspect,
                    Insights = executedCell?.Result ?? "No insights generated",
                    GeneratedAt = DateTime.UtcNow
                };
                
                return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error generating campaign insights: {ex.Message}";
            }
        }
    }
} 