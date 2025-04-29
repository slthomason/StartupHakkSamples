using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using ModelContextProtocol.Server;

namespace MCPServer.CSharp
{
    [McpServerToolType]
    public static class CrawlerTool
    {
        private static readonly HashSet<string> _visitedUrls = new HashSet<string>();
        private static readonly Queue<string> _urlQueue = new Queue<string>();
        private static bool _isCrawling = false;
        
        [McpServerTool, Description("Starts cataloging the internet from seed URLs, discovering and analyzing connected sites.")]
        public static string StartCatalogCrawler(
            CrawlerService crawlerService,
            [Description("Comma-separated list of seed URLs to start crawling from")] string seedUrls,
            [Description("Maximum number of links to follow from each page (default: 5)")] int maxLinksPerPage = 5,
            [Description("Maximum depth to crawl (default: 3)")] int maxDepth = 3)
        {
            try
            {
                if (_isCrawling)
                {
                    return "A cataloging job is already in progress. Use StopCatalogCrawler to stop it first.";
                }
                
                var urls = seedUrls.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(u => u.Trim())
                    .Where(u => !string.IsNullOrEmpty(u))
                    .ToList();
                    
                if (urls.Count == 0)
                {
                    return "No valid seed URLs provided. Please specify at least one URL to start crawling.";
                }
                
                // Clear previous state
                _visitedUrls.Clear();
                _urlQueue.Clear();
                
                // Add seed URLs to the queue
                foreach (var url in urls)
                {
                    _urlQueue.Enqueue(url + "|0"); // Format: url|depth
                }
                
                _isCrawling = true;
                
                // Start crawling asynchronously
                Task.Run(() => CrawlWebsites(crawlerService, maxLinksPerPage, maxDepth));
                
                return $"Started internet cataloging from {urls.Count} seed URLs. Crawling to maximum depth {maxDepth} with {maxLinksPerPage} max links per page.";
            }
            catch (Exception ex)
            {
                return $"Error starting catalog crawler: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Stops any currently running internet cataloging job.")]
        public static string StopCatalogCrawler()
        {
            if (!_isCrawling)
            {
                return "No cataloging job is currently running.";
            }
            
            _isCrawling = false;
            return "Stopping internet cataloging. The current job will finish its current page and then terminate.";
        }
        
        [McpServerTool, Description("Gets the current status of the internet cataloging process.")]
        public static string GetCatalogStatus(CrawlerService crawlerService)
        {
            var analyzedCount = _visitedUrls.Count;
            var queuedCount = _urlQueue.Count;
            
            var status = new
            {
                IsRunning = _isCrawling,
                AnalyzedUrlCount = analyzedCount,
                QueuedUrlCount = queuedCount,
                TotalCatalogSize = crawlerService.GetCatalogSize(),
                Categories = crawlerService.GetAvailableCategories()
                    .ToDictionary(c => c, c => crawlerService.GetCatalogEntriesAsync(c).GetAwaiter().GetResult().Count)
            };
            
            return JsonSerializer.Serialize(status, new JsonSerializerOptions { WriteIndented = true });
        }
        
        [McpServerTool, Description("Imports a list of URLs to analyze in batch mode.")]
        public static async Task<string> ImportUrlList(
            CrawlerService crawlerService,
            [Description("Newline-separated list of URLs to analyze")] string urlList)
        {
            try
            {
                var urls = urlList.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(u => u.Trim())
                    .Where(u => !string.IsNullOrEmpty(u))
                    .ToList();
                    
                if (urls.Count == 0)
                {
                    return "No valid URLs provided.";
                }
                
                Console.WriteLine($"Batch analyzing {urls.Count} URLs...");
                
                int analyzed = 0;
                var results = new List<WebsiteCategory>();
                
                foreach (var url in urls)
                {
                    try
                    {
                        var result = await crawlerService.AnalyzeWebsiteAsync(url);
                        results.Add(result);
                        analyzed++;
                        
                        if (analyzed % 10 == 0)
                        {
                            Console.WriteLine($"Analyzed {analyzed}/{urls.Count} URLs...");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error analyzing {url}: {ex.Message}");
                    }
                }
                
                var summary = new
                {
                    TotalUrls = urls.Count,
                    SuccessfullyAnalyzed = analyzed,
                    Failed = urls.Count - analyzed,
                    CategoriesFound = results.Select(r => r.Category).Distinct().ToList(),
                    UnsafeCount = results.Count(r => !r.IsSafeForChildren)
                };
                
                return JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error batch importing URLs: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Export the current catalog to a JSON file.")]
        public static string ExportCatalog(
            CrawlerService crawlerService,
            [Description("Path to save the catalog export")] string filePath)
        {
            try
            {
                var result = crawlerService.SaveCatalogToFileAsync(filePath).GetAwaiter().GetResult();
                
                if (result)
                {
                    return $"Successfully exported Internet catalog to {filePath}";
                }
                else
                {
                    return $"Failed to export catalog to {filePath}";
                }
            }
            catch (Exception ex)
            {
                return $"Error exporting catalog: {ex.Message}";
            }
        }
        
        [McpServerTool, Description("Generates a summary report of the catalog.")]
        public static string GenerateCatalogReport(CrawlerService crawlerService)
        {
            try
            {
                var categories = crawlerService.GetAvailableCategories();
                var categoryStats = new Dictionary<string, object>();
                
                foreach (var category in categories)
                {
                    var urls = crawlerService.GetCatalogEntriesAsync(category).GetAwaiter().GetResult();
                    
                    // Get safe vs unsafe stats
                    var safeCount = 0;
                    var unsafeCount = 0;
                    
                    foreach (var url in urls)
                    {
                        var details = crawlerService.GetWebsiteDetailsAsync(url).GetAwaiter().GetResult();
                        if (details != null)
                        {
                            if (details.IsSafeForChildren)
                                safeCount++;
                            else
                                unsafeCount++;
                        }
                    }
                    
                    categoryStats[category] = new
                    {
                        Count = urls.Count,
                        SafeCount = safeCount,
                        UnsafeCount = unsafeCount,
                        PercentSafe = urls.Count > 0 ? Math.Round(100.0 * safeCount / urls.Count, 1) : 0
                    };
                }
                
                var report = new
                {
                    TotalSites = crawlerService.GetCatalogSize(),
                    CategoryBreakdown = categoryStats,
                    MostCommonKeywords = crawlerService.GetMostCommonKeywords(10),
                    GeneratedAt = DateTime.UtcNow
                };
                
                return JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                return $"Error generating catalog report: {ex.Message}";
            }
        }
        
        private static async Task CrawlWebsites(CrawlerService crawlerService, int maxLinksPerPage, int maxDepth)
        {
            Console.WriteLine("Starting web crawler for internet cataloging...");
            
            try
            {
                while (_isCrawling && _urlQueue.Count > 0)
                {
                    // Get the next URL from the queue
                    var item = _urlQueue.Dequeue();
                    var parts = item.Split('|');
                    var url = parts[0];
                    var depth = int.Parse(parts[1]);
                    
                    // Skip if we've already visited this URL
                    if (_visitedUrls.Contains(url))
                    {
                        continue;
                    }
                    
                    // Mark as visited
                    _visitedUrls.Add(url);
                    
                    Console.WriteLine($"Analyzing {url} (depth {depth}/{maxDepth})");
                    
                    try
                    {
                        // Analyze the website
                        var result = await crawlerService.AnalyzeWebsiteAsync(url);
                        
                        // If we haven't reached max depth, enqueue linked URLs
                        if (depth < maxDepth)
                        {
                            int linksAdded = 0;
                            
                            // Get links from the result
                            foreach (var linkedUrl in result.LinkedUrls)
                            {
                                if (!_visitedUrls.Contains(linkedUrl) && 
                                    !_urlQueue.Any(q => q.StartsWith(linkedUrl + "|")))
                                {
                                    _urlQueue.Enqueue(linkedUrl + "|" + (depth + 1));
                                    linksAdded++;
                                    
                                    if (linksAdded >= maxLinksPerPage)
                                        break;
                                }
                            }
                            
                            Console.WriteLine($"Added {linksAdded} new URLs to the queue from {url}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {url}: {ex.Message}");
                    }
                    
                    // Small delay to avoid overloading
                    await Task.Delay(1000);
                }
                
                Console.WriteLine("Web crawling complete or stopped.");
                _isCrawling = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in web crawler: {ex.Message}");
                _isCrawling = false;
            }
        }
    }
} 