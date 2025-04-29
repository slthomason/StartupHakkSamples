using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using ModelContextProtocol.Server;
using MCPServer.CSharp;

namespace MCPServer.CSharp
{
    [McpServerToolType]
    public static class CampaignReportTool
    {
        [McpServerTool, Description("Lists all available campaign IDs with names.")]
        public static string ListCampaigns(
            NotebookService notebookService)
        {
            try
            {
                var campaigns = notebookService.GetAllCampaigns();
                
                var summaries = campaigns.Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    CallCount = c.Calls.Count
                });
                
                return JsonSerializer.Serialize(summaries, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error listing campaigns: {ex.Message}";
            }
        }

        [McpServerTool, Description("Generates a JSON analytics report for a campaign.")]
        public static string GenerateCampaignReport(
            NotebookService notebookService,
            [Description("ID of the campaign to report on")] string campaignId)
        {
            try
            {
                var campaign = notebookService.GetCampaign(campaignId);
                if (campaign == null)
                    return $"Campaign '{campaignId}' not found.";

                var calls = campaign.Calls;
                var totalCalls = calls.Count;
                var avgDuration = calls.Any()
                    ? calls.Average(c => c.DurationSeconds)
                    : 0.0;

                var byStatus = calls
                    .GroupBy(c => c.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() });

                var byDisposition = calls
                    .Where(c => !string.IsNullOrEmpty(c.DispositionCode))
                    .GroupBy(c => c.DispositionCode)
                    .Select(g => new { Disposition = g.Key, Count = g.Count() });

                var byAgent = calls
                    .Where(c => !string.IsNullOrEmpty(c.AgentId))
                    .GroupBy(c => c.AgentId)
                    .Select(g => new { AgentId = g.Key, Count = g.Count() });

                // Calculate call duration percentiles for distribution analysis
                var durations = calls.Select(c => c.DurationSeconds).OrderBy(d => d).ToList();
                double median = 0;
                if (durations.Count > 0)
                {
                    median = durations.Count % 2 == 0
                        ? (durations[durations.Count / 2 - 1] + durations[durations.Count / 2]) / 2
                        : durations[durations.Count / 2];
                }

                var report = new
                {
                    Id = campaign.Id,
                    Name = campaign.Name,
                    Description = campaign.Description,
                    StartDate = campaign.StartDate.ToString("yyyy-MM-dd"),
                    RunningDays = (DateTime.UtcNow - campaign.StartDate).Days,
                    TotalCalls = totalCalls,
                    CallsPerDay = Math.Round(totalCalls / Math.Max(1, (DateTime.UtcNow - campaign.StartDate).TotalDays), 1),
                    AverageCallDurationSeconds = Math.Round(avgDuration, 1),
                    MedianCallDurationSeconds = Math.Round(median, 1),
                    CallsByStatus = byStatus,
                    DispositionSummary = byDisposition,
                    AgentPerformance = byAgent,
                    ReportGeneratedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                };

                return JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error in CampaignReportTool: {ex.Message}";
            }
        }
    }
} 