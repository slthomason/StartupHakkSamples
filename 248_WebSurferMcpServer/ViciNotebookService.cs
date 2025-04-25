using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace MCPServer.CSharp
{
    public class CampaignCall
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Status { get; set; } = string.Empty;
        public string DispositionCode { get; set; } = string.Empty;
        public string AgentId { get; set; } = string.Empty;
        public double DurationSeconds { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class Campaign 
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<CampaignCall> Calls { get; set; } = new List<CampaignCall>();
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public enum NotebookCellType
    {
        Markdown,
        Data,
        Chart,
        Insight
    }

    public class NotebookCell
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public NotebookCellType Type { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Result { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExecutedAt { get; set; }
    }

    public class AnalysisNotebook
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string CampaignId { get; set; } = string.Empty;
        public List<NotebookCell> Cells { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ViciNotebookService
    {
        private readonly ILogger<ViciNotebookService> _logger;
        private readonly Dictionary<string, Campaign> _campaigns = new Dictionary<string, Campaign>();
        private readonly Dictionary<string, AnalysisNotebook> _notebooks = new Dictionary<string, AnalysisNotebook>();

        public ViciNotebookService(ILogger<ViciNotebookService> logger)
        {
            _logger = logger;
            // Create demo campaign data
            CreateDemoCampaigns();
            // Create a demo notebook
            CreateDemoNotebook();
        }

        private void CreateDemoCampaigns()
        {
            // Demo campaign 1 - Successful customer feedback
            var campaign1 = new Campaign
            {
                Id = "CAMP-001",
                Name = "Customer Satisfaction Survey Q2",
                Description = "Follow-up survey for recent product purchases",
                StartDate = DateTime.UtcNow.AddDays(-30)
            };

            // Add some sample calls to the campaign
            var statuses = new[] { "COMPLETED", "NO_ANSWER", "BUSY", "VOICEMAIL", "REJECTED" };
            var dispositions = new[] { "SATISFIED", "NEUTRAL", "DISSATISFIED", "REQUESTED_CALLBACK", "NOT_INTERESTED" };
            var agents = new[] { "agent001", "agent002", "agent003" };
            
            var random = new Random(123); // Fixed seed for reproducible demo data
            
            for (int i = 0; i < 50; i++)
            {
                campaign1.Calls.Add(new CampaignCall
                {
                    Status = statuses[random.Next(statuses.Length)],
                    DispositionCode = dispositions[random.Next(dispositions.Length)],
                    AgentId = agents[random.Next(agents.Length)],
                    DurationSeconds = random.Next(30, 600),
                    PhoneNumber = $"+1555{random.Next(1000000, 9999999)}",
                    Timestamp = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                });
            }

            // Demo campaign 2 - Sales outreach
            var campaign2 = new Campaign
            {
                Id = "CAMP-002",
                Name = "Enterprise Sales Outreach",
                Description = "Introducing our new enterprise solution",
                StartDate = DateTime.UtcNow.AddDays(-15)
            };
            
            var salesDispositions = new[] { "INTERESTED", "FOLLOW_UP", "PURCHASED", "NOT_QUALIFIED", "COMPETITOR" };
            
            for (int i = 0; i < 35; i++)
            {
                campaign2.Calls.Add(new CampaignCall
                {
                    Status = statuses[random.Next(statuses.Length)],
                    DispositionCode = salesDispositions[random.Next(salesDispositions.Length)],
                    AgentId = agents[random.Next(agents.Length)],
                    DurationSeconds = random.Next(120, 1200),
                    PhoneNumber = $"+1555{random.Next(1000000, 9999999)}",
                    Timestamp = DateTime.UtcNow.AddDays(-random.Next(1, 15))
                });
            }

            // Store campaigns
            _campaigns[campaign1.Id] = campaign1;
            _campaigns[campaign2.Id] = campaign2;
            
            _logger.LogInformation("Created {Count} demo campaigns", _campaigns.Count);
        }

        private void CreateDemoNotebook()
        {
            // Create a sample analysis notebook for the first campaign
            if (_campaigns.Count > 0)
            {
                var campaignId = "CAMP-001";
                var notebook = new AnalysisNotebook
                {
                    Title = "Q2 Survey Analysis",
                    CampaignId = campaignId,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                };
                
                // Add some cells to the notebook
                notebook.Cells.Add(new NotebookCell
                {
                    Type = NotebookCellType.Markdown,
                    Content = "# Customer Satisfaction Survey Analysis\n\nThis notebook analyzes the results of our Q2 customer satisfaction survey campaign.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                });
                
                notebook.Cells.Add(new NotebookCell
                {
                    Type = NotebookCellType.Data,
                    Content = $"SELECT * FROM Calls WHERE CampaignId = '{campaignId}' LIMIT 10",
                    Result = JsonSerializer.Serialize(_campaigns[campaignId].Calls.Take(10)),
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    ExecutedAt = DateTime.UtcNow.AddDays(-2)
                });
                
                notebook.Cells.Add(new NotebookCell
                {
                    Type = NotebookCellType.Chart,
                    Content = "BAR CHART: Call Dispositions",
                    Result = "chart-data-placeholder",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    ExecutedAt = DateTime.UtcNow.AddDays(-1)
                });
                
                notebook.Cells.Add(new NotebookCell
                {
                    Type = NotebookCellType.Insight,
                    Content = "ANALYZE: What are the main reasons for customer dissatisfaction?",
                    Result = "Based on call analysis, the top 3 factors for dissatisfaction are: 1) Long wait times (mentioned in 68% of calls), 2) Product complexity (52%), and 3) Pricing concerns (47%).",
                    CreatedAt = DateTime.UtcNow.AddHours(-12),
                    ExecutedAt = DateTime.UtcNow.AddHours(-12)
                });
                
                _notebooks[notebook.Id] = notebook;
                _logger.LogInformation("Created demo notebook with ID {Id}", notebook.Id);
            }
        }

        public Campaign GetCampaign(string campaignId)
        {
            if (_campaigns.TryGetValue(campaignId, out var campaign))
            {
                return campaign;
            }
            
            _logger.LogWarning("Campaign with ID {Id} not found", campaignId);
            return null;
        }

        public List<Campaign> GetAllCampaigns()
        {
            return new List<Campaign>(_campaigns.Values);
        }

        // Notebook methods
        public List<AnalysisNotebook> GetNotebooks()
        {
            return _notebooks.Values.ToList();
        }
        
        public AnalysisNotebook GetNotebook(string notebookId)
        {
            return _notebooks.TryGetValue(notebookId, out var notebook) ? notebook : null;
        }
        
        public AnalysisNotebook CreateNotebook(string title, string campaignId)
        {
            var notebook = new AnalysisNotebook
            {
                Title = title,
                CampaignId = campaignId,
            };
            
            _notebooks[notebook.Id] = notebook;
            return notebook;
        }
        
        public NotebookCell AddCell(string notebookId, NotebookCellType cellType, string content)
        {
            if (!_notebooks.TryGetValue(notebookId, out var notebook))
            {
                _logger.LogWarning("Notebook with ID {Id} not found", notebookId);
                return null;
            }
            
            var cell = new NotebookCell
            {
                Type = cellType,
                Content = content
            };
            
            notebook.Cells.Add(cell);
            notebook.UpdatedAt = DateTime.UtcNow;
            
            return cell;
        }
        
        public async Task<NotebookCell> ExecuteCell(string notebookId, string cellId)
        {
            if (!_notebooks.TryGetValue(notebookId, out var notebook))
            {
                _logger.LogWarning("Notebook with ID {Id} not found", notebookId);
                return null;
            }
            
            var cell = notebook.Cells.FirstOrDefault(c => c.Id == cellId);
            if (cell == null)
            {
                _logger.LogWarning("Cell with ID {Id} not found in notebook {NotebookId}", cellId, notebookId);
                return null;
            }
            
            // Execute the cell based on its type
            try
            {
                switch (cell.Type)
                {
                    case NotebookCellType.Data:
                        // Simulate a SQL query execution
                        if (cell.Content.Contains("SELECT") && cell.Content.Contains("FROM Calls"))
                        {
                            if (!_campaigns.TryGetValue(notebook.CampaignId, out var campaign))
                            {
                                cell.Result = "Error: Campaign not found";
                                break;
                            }
                            
                            // Very simplistic "query parsing" - just a demo!
                            int limit = 100;
                            if (cell.Content.Contains("LIMIT"))
                            {
                                var limitMatch = System.Text.RegularExpressions.Regex.Match(cell.Content, @"LIMIT\s+(\d+)");
                                if (limitMatch.Success)
                                {
                                    limit = int.Parse(limitMatch.Groups[1].Value);
                                }
                            }
                            
                            cell.Result = JsonSerializer.Serialize(campaign.Calls.Take(limit));
                        }
                        else
                        {
                            cell.Result = "Unsupported query format";
                        }
                        break;
                        
                    case NotebookCellType.Chart:
                        // Simulate chart generation
                        cell.Result = "chart-data-placeholder";
                        break;
                        
                    case NotebookCellType.Insight:
                        // Simulate LLM analysis
                        if (cell.Content.StartsWith("ANALYZE:"))
                        {
                            var query = cell.Content.Substring("ANALYZE:".Length).Trim();
                            if (query.Contains("dissatisfaction"))
                            {
                                cell.Result = "Based on call analysis, the top 3 factors for dissatisfaction are: 1) Long wait times (mentioned in 68% of calls), 2) Product complexity (52%), and 3) Pricing concerns (47%).";
                            }
                            else if (query.Contains("satisfaction") || query.Contains("satisfied"))
                            {
                                cell.Result = "Customer satisfaction analysis: 65% of customers reported being 'very satisfied' with their experience, citing helpful agents (78%) and quick resolution (82%) as the primary factors.";
                            }
                            else
                            {
                                cell.Result = "Analysis complete. Key findings: 1) Call resolution time averages 4.5 minutes, 2) Agent performance is consistent across team members, 3) Morning calls have a 22% higher satisfaction rate than afternoon calls.";
                            }
                        }
                        else
                        {
                            cell.Result = "Use format 'ANALYZE: your question' to perform AI analysis";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                cell.Result = $"Error executing cell: {ex.Message}";
                _logger.LogError(ex, "Error executing cell {CellId} in notebook {NotebookId}", cellId, notebookId);
            }
            
            cell.ExecutedAt = DateTime.UtcNow;
            notebook.UpdatedAt = DateTime.UtcNow;
            
            return cell;
        }
    }
} 