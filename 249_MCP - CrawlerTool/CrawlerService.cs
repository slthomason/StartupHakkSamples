using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;

namespace MCPServer.CSharp
{
    public class WebsiteCategory
    {
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = new List<string>();
        public List<string> LinkedUrls { get; set; } = new List<string>();
        public bool IsSafeForChildren { get; set; }
        public DateTime AnalyzedAt { get; set; } = DateTime.UtcNow;
    }

    public class CrawlerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CrawlerService> _logger;
        
        // In-memory catalog (in production, this would be a database)
        private readonly Dictionary<string, WebsiteCategory> _websiteCatalog = new();
        private readonly Dictionary<string, List<string>> _categoryUrlMap = new Dictionary<string, List<string>>();
        private readonly Dictionary<string, int> _keywordFrequency = new Dictionary<string, int>();

        private bool _isCrawling = false;
        private int _maxLinksPerPage = 10;
        private int _maxDepth = 2;
        private HashSet<string> _urlQueue = new HashSet<string>();
        private HashSet<string> _processedUrls = new HashSet<string>();

        public CrawlerService(IHttpClientFactory httpClientFactory, ILogger<CrawlerService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            
            // Set a user agent to avoid being blocked
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CleanRouterCrawler/1.0");
            
            // Initialize with some default categories
            _categoryUrlMap["Education"] = new List<string>();
            _categoryUrlMap["News"] = new List<string>();
            _categoryUrlMap["Entertainment"] = new List<string>();
            _categoryUrlMap["Technology"] = new List<string>();
            _categoryUrlMap["Business"] = new List<string>();
            _categoryUrlMap["Health"] = new List<string>();
            _categoryUrlMap["Science"] = new List<string>();
            _categoryUrlMap["Other"] = new List<string>();
            
            // Seed with some pre-analyzed sites for the demo
            SeedCatalog();
        }
        
        private void SeedCatalog()
        {
            // News site example
            _websiteCatalog["https://www.cnn.com"] = new WebsiteCategory
            {
                Url = "https://www.cnn.com",
                Title = "CNN - Breaking News, Latest News and Videos",
                Category = "News",
                Keywords = new List<string> { "news", "politics", "world", "breaking news", "us news" },
                IsSafeForChildren = true,
                AnalyzedAt = DateTime.UtcNow.AddDays(-5)
            };
            
            // Social media example
            _websiteCatalog["https://www.twitter.com"] = new WebsiteCategory
            {
                Url = "https://www.twitter.com",
                Title = "Twitter / X",
                Category = "Social Media",
                Keywords = new List<string> { "social media", "tweets", "social network", "microblogging" },
                IsSafeForChildren = false, // Due to unmoderated content
                AnalyzedAt = DateTime.UtcNow.AddDays(-3)
            };
            
            // Education example
            _websiteCatalog["https://www.wikipedia.org"] = new WebsiteCategory
            {
                Url = "https://www.wikipedia.org",
                Title = "Wikipedia - The Free Encyclopedia",
                Category = "Education",
                Keywords = new List<string> { "encyclopedia", "knowledge", "articles", "research", "education" },
                IsSafeForChildren = true,
                AnalyzedAt = DateTime.UtcNow.AddDays(-7)
            };
            
            // Tech example
            _websiteCatalog["https://www.github.com"] = new WebsiteCategory
            {
                Url = "https://www.github.com",
                Title = "GitHub: Let's build from here",
                Category = "Technology",
                Keywords = new List<string> { "code", "git", "software", "development", "repository", "programming" },
                IsSafeForChildren = true,
                AnalyzedAt = DateTime.UtcNow.AddDays(-2)
            };
            
            _logger.LogInformation("Seeded catalog with {Count} websites", _websiteCatalog.Count);
        }

        public async Task<WebsiteCategory> AnalyzeWebsiteAsync(string url)
        {
            try
            {
                _logger.LogInformation("Analyzing website: {Url}", url);
                
                // Normalize URL
                url = NormalizeUrl(url);
                
                // Check cache first
                if (_websiteCatalog.TryGetValue(url, out var cachedCategory))
                {
                    _logger.LogInformation("Found cached analysis for {Url}", url);
                    return cachedCategory;
                }

                // For demo purposes - if URL isn't in our seed data, generate a simulated analysis
                // In a real implementation, this would fetch and analyze actual content
                
                // Extract domain for better simulated analysis
                var domain = new Uri(url).Host.ToLower();
                
                var result = new WebsiteCategory
                {
                    Url = url,
                    Title = $"Website at {domain}",
                    LinkedUrls = new List<string>()
                };
                
                // Simulate category and safety based on domain keywords
                if (domain.Contains("news") || domain.Contains("cnn") || domain.Contains("bbc"))
                {
                    result.Category = "News";
                    result.Keywords = new List<string> { "news", "articles", "reporting", "journalism" };
                    result.IsSafeForChildren = true;
                }
                else if (domain.Contains("game") || domain.Contains("play"))
                {
                    result.Category = "Gaming";
                    result.Keywords = new List<string> { "games", "entertainment", "gaming", "play" };
                    result.IsSafeForChildren = domain.Contains("adult") ? false : true;
                }
                else if (domain.Contains("edu") || domain.Contains("learn") || domain.Contains("course"))
                {
                    result.Category = "Education";
                    result.Keywords = new List<string> { "education", "learning", "courses", "academic" };
                    result.IsSafeForChildren = true;
                }
                else if (domain.Contains("shop") || domain.Contains("buy") || domain.Contains("store"))
                {
                    result.Category = "Shopping";
                    result.Keywords = new List<string> { "shopping", "products", "store", "ecommerce" };
                    result.IsSafeForChildren = true;
                }
                else if (domain.Contains("social") || domain.Contains("community") || 
                         domain.Contains("facebook") || domain.Contains("twitter") || 
                         domain.Contains("instagram"))
                {
                    result.Category = "Social Media";
                    result.Keywords = new List<string> { "social", "communication", "sharing", "community" };
                    result.IsSafeForChildren = false; // Social media usually has unmoderated content
                }
                else
                {
                    // Default fallback
                    result.Category = "Uncategorized";
                    result.Keywords = new List<string> { "website", "internet" };
                    result.IsSafeForChildren = false; // Conservative default
                }
                
                // Store in catalog
                _websiteCatalog[url] = result;
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing website {Url}", url);
                
                // Return a basic fallback result
                return new WebsiteCategory
                {
                    Url = url,
                    Title = "Error analyzing website",
                    Category = "Error",
                    Keywords = new List<string> { "error" },
                    IsSafeForChildren = false
                };
            }
        }

        public async Task<List<string>> GetCatalogEntriesAsync(string categoryFilter = "")
        {
            // Return URLs of websites in catalog, optionally filtered by category
            if (string.IsNullOrEmpty(categoryFilter))
            {
                return _websiteCatalog.Keys.ToList();
            }
            else
            {
                return _websiteCatalog
                    .Where(kv => kv.Value.Category.Equals(categoryFilter, StringComparison.OrdinalIgnoreCase))
                    .Select(kv => kv.Key)
                    .ToList();
            }
        }
        
        public int GetCatalogSize()
        {
            return _websiteCatalog.Count;
        }
        
        public List<string> GetAvailableCategories()
        {
            return _categoryUrlMap.Keys.ToList();
        }
        
        public async Task<WebsiteCategory> GetWebsiteDetailsAsync(string url)
        {
            if (_websiteCatalog.TryGetValue(url, out var category))
            {
                return category;
            }
            
            return null;
        }
        
        public async Task<bool> SaveCatalogToFileAsync(string filePath)
        {
            try
            {
                var catalog = new
                {
                    GeneratedAt = DateTime.UtcNow,
                    TotalSites = GetCatalogSize(),
                    Categories = GetAvailableCategories().ToDictionary(
                        cat => cat,
                        cat => GetUrlsByCategory(cat).ToList()
                    ),
                    WebsiteDetails = _websiteCatalog
                };
                
                string json = JsonSerializer.Serialize(catalog, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving catalog to file {FilePath}", filePath);
                return false;
            }
        }
        
        public Dictionary<string, int> GetMostCommonKeywords(int count)
        {
            var keywordFrequency = new Dictionary<string, int>();
            
            // Calculate keyword frequency
            foreach (var site in _websiteCatalog.Values)
            {
                foreach (var keyword in site.Keywords)
                {
                    if (keywordFrequency.ContainsKey(keyword))
                    {
                        keywordFrequency[keyword]++;
                    }
                    else
                    {
                        keywordFrequency[keyword] = 1;
                    }
                }
            }
            
            return keywordFrequency
                .OrderByDescending(kv => kv.Value)
                .Take(count)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public async Task<bool> StartCrawlingAsync(List<string> seedUrls, int maxLinksPerPage = 10, int maxDepth = 2)
        {
            if (_isCrawling)
            {
                return false;
            }
            
            _isCrawling = true;
            _maxLinksPerPage = maxLinksPerPage;
            _maxDepth = maxDepth;
            _urlQueue = new HashSet<string>(seedUrls.Select(url => NormalizeUrl(url)));
            
            // Start crawling in background
            _ = Task.Run(async () => await CrawlWebsitesAsync());
            
            return true;
        }
        
        public bool StopCrawling()
        {
            if (!_isCrawling)
            {
                return false;
            }
            
            _isCrawling = false;
            return true;
        }
        
        public (bool IsCrawling, int ProcessedCount, int QueuedCount) GetCrawlingStatus()
        {
            return (_isCrawling, _processedUrls.Count, _urlQueue.Count);
        }
        
        public async Task<(int Processed, int Added, int Skipped)> ImportUrlListAsync(List<string> urls)
        {
            int processed = 0;
            int added = 0;
            int skipped = 0;
            
            foreach (var url in urls)
            {
                processed++;
                string normalizedUrl = NormalizeUrl(url);
                
                if (_websiteCatalog.ContainsKey(normalizedUrl))
                {
                    skipped++;
                    continue;
                }
                
                try
                {
                    await AnalyzeWebsiteAsync(normalizedUrl);
                    added++;
                }
                catch (Exception)
                {
                    skipped++;
                }
            }
            
            return (processed, added, skipped);
        }
        
        public string GenerateCatalogReport()
        {
            var report = new StringBuilder();
            
            report.AppendLine($"Clean Router Catalog Report - Generated on {DateTime.UtcNow}");
            report.AppendLine("===========================================================");
            report.AppendLine();
            
            report.AppendLine($"Total websites indexed: {_websiteCatalog.Count}");
            
            // Category breakdown
            report.AppendLine("\nCategory Breakdown:");
            report.AppendLine("------------------");
            
            var categories = GetAvailableCategories();
            foreach (var category in categories)
            {
                var urlsInCategory = GetUrlsByCategory(category).Count();
                report.AppendLine($"{category}: {urlsInCategory} websites ({(double)urlsInCategory / _websiteCatalog.Count:P1})");
            }
            
            // Safety stats
            report.AppendLine("\nSafety Statistics:");
            report.AppendLine("------------------");
            
            int safeWebsites = _websiteCatalog.Values.Count(w => w.IsSafeForChildren);
            report.AppendLine($"Safe for children: {safeWebsites} websites ({(double)safeWebsites / _websiteCatalog.Count:P1})");
            report.AppendLine($"Not safe for children: {_websiteCatalog.Count - safeWebsites} websites ({(double)(_websiteCatalog.Count - safeWebsites) / _websiteCatalog.Count:P1})");
            
            // Top keywords
            report.AppendLine("\nTop Keywords:");
            report.AppendLine("------------------");
            
            var topKeywords = GetMostCommonKeywords(10);
            foreach (var keyword in topKeywords)
            {
                report.AppendLine($"{keyword.Key}: {keyword.Value} occurrences");
            }
            
            // Recent sites
            report.AppendLine("\nRecently Added Websites:");
            report.AppendLine("------------------");
            
            var recentSites = _websiteCatalog.Values
                .OrderByDescending(w => w.AnalyzedAt)
                .Take(5);
                
            foreach (var site in recentSites)
            {
                report.AppendLine($"{site.Title} - {site.Url} (Added: {site.AnalyzedAt})");
            }
            
            return report.ToString();
        }
        
        private async Task CrawlWebsitesAsync()
        {
            _processedUrls = new HashSet<string>();
            Dictionary<string, int> urlDepth = new Dictionary<string, int>();
            
            // Initialize starting URLs with depth 0
            foreach (var url in _urlQueue)
            {
                urlDepth[url] = 0;
            }
            
            while (_isCrawling && _urlQueue.Count > 0)
            {
                // Get next URL from queue
                string url = _urlQueue.First();
                _urlQueue.Remove(url);
                
                // Skip if already processed
                if (_processedUrls.Contains(url))
                {
                    continue;
                }
                
                // Process the URL
                try
                {
                    int currentDepth = urlDepth[url];
                    var websiteDetails = await AnalyzeWebsiteAsync(url);
                    _processedUrls.Add(url);
                    
                    // Only add new links if we haven't reached max depth
                    if (currentDepth < _maxDepth)
                    {
                        // Add new links to queue with incremented depth
                        var linksToAdd = websiteDetails.LinkedUrls
                            .Take(_maxLinksPerPage)
                            .Where(link => !_processedUrls.Contains(link) && !_urlQueue.Contains(link));
                            
                        foreach (var link in linksToAdd)
                        {
                            _urlQueue.Add(link);
                            urlDepth[link] = currentDepth + 1;
                        }
                    }
                    
                    // Small delay to avoid overloading servers
                    await Task.Delay(500);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing URL {Url} during crawling", url);
                    // Continue with next URL
                }
            }
            
            _isCrawling = false;
        }

        private IEnumerable<string> GetUrlsByCategory(string category)
        {
            return _websiteCatalog.Values
                .Where(w => w.Category == category)
                .Select(w => w.Url);
        }

        #region Helper Methods
        
        private string NormalizeUrl(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }
            
            // Remove trailing slash
            if (url.EndsWith("/"))
            {
                url = url.Substring(0, url.Length - 1);
            }
            
            // Remove query parameters for simplicity
            int queryIndex = url.IndexOf('?');
            if (queryIndex > 0)
            {
                url = url.Substring(0, queryIndex);
            }
            
            return url;
        }
        
        private string ExtractTitle(string html)
        {
            var titleMatch = Regex.Match(html, @"<title>(.*?)</title>");
            if (titleMatch.Success)
            {
                return CleanHtml(titleMatch.Groups[1].Value);
            }
            return null;
        }
        
        private string ExtractMetaDescription(string html)
        {
            var metaMatch = Regex.Match(html, @"<meta\s+name=""description""\s+content=""(.*?)""", RegexOptions.IgnoreCase);
            if (metaMatch.Success)
            {
                return CleanHtml(metaMatch.Groups[1].Value);
            }
            return null;
        }
        
        private List<string> ExtractKeywords(string html)
        {
            // Try meta keywords
            var metaMatch = Regex.Match(html, @"<meta\s+name=""keywords""\s+content=""(.*?)""", RegexOptions.IgnoreCase);
            if (metaMatch.Success)
            {
                return metaMatch.Groups[1].Value
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.Trim().ToLower())
                    .Where(k => !string.IsNullOrEmpty(k))
                    .ToList();
            }
            
            // Extract headings as keywords
            var headings = new List<string>();
            var h1Matches = Regex.Matches(html, @"<h1[^>]*>(.*?)</h1>", RegexOptions.IgnoreCase);
            var h2Matches = Regex.Matches(html, @"<h2[^>]*>(.*?)</h2>", RegexOptions.IgnoreCase);
            
            foreach (Match match in h1Matches)
            {
                headings.Add(CleanHtml(match.Groups[1].Value));
            }
            
            foreach (Match match in h2Matches)
            {
                headings.Add(CleanHtml(match.Groups[1].Value));
            }
            
            // Extract words from headings
            var keywords = new HashSet<string>();
            foreach (var heading in headings)
            {
                var words = heading.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?' },
                    StringSplitOptions.RemoveEmptyEntries)
                    .Select(w => w.ToLower())
                    .Where(w => w.Length > 3); // Only words longer than 3 chars
                
                foreach (var word in words)
                {
                    keywords.Add(word);
                }
            }
            
            return keywords.Take(20).ToList(); // Limit to 20 keywords
        }
        
        private List<string> ExtractLinks(string html, string baseUrl)
        {
            var links = new HashSet<string>();
            var matches = Regex.Matches(html, @"<a\s+(?:[^>]*?\s+)?href=""([^""]*)""", RegexOptions.IgnoreCase);
            
            foreach (Match match in matches)
            {
                string href = match.Groups[1].Value;
                
                // Skip empty, javascript, mailto and anchor links
                if (string.IsNullOrEmpty(href) ||
                    href.StartsWith("javascript:") ||
                    href.StartsWith("mailto:") ||
                    href.StartsWith("#"))
                {
                    continue;
                }
                
                // Normalize URL
                if (href.StartsWith("/"))
                {
                    // Convert relative URL to absolute
                    Uri baseUri = new Uri(baseUrl);
                    href = new Uri(baseUri, href).ToString();
                }
                else if (!href.StartsWith("http://") && !href.StartsWith("https://"))
                {
                    // Might be a relative path without leading slash
                    Uri baseUri = new Uri(baseUrl);
                    href = new Uri(baseUri, href).ToString();
                }
                
                links.Add(NormalizeUrl(href));
            }
            
            return links.Take(50).ToList(); // Limit to 50 links
        }
        
        private string CleanHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
                
            // Remove HTML tags
            text = Regex.Replace(text, @"<[^>]+>", " ");
            
            // Decode HTML entities
            text = System.Net.WebUtility.HtmlDecode(text);
            
            // Normalize whitespace
            text = Regex.Replace(text, @"\s+", " ").Trim();
            
            return text;
        }
        
        private string ClassifyWebsite(string title, string description, List<string> keywords)
        {
            // Simple classification based on keywords
            var categoryKeywords = new Dictionary<string, List<string>>
            {
                ["Education"] = new List<string> { "university", "college", "school", "course", "learn", "education", "student", "academic", "study", "knowledge" },
                ["News"] = new List<string> { "news", "latest", "breaking", "headlines", "report", "update", "world", "politics", "economy" },
                ["Entertainment"] = new List<string> { "entertainment", "movie", "music", "game", "play", "fun", "stream", "video", "watch", "listen" },
                ["Technology"] = new List<string> { "technology", "tech", "software", "hardware", "programming", "developer", "code", "digital", "computer", "app" },
                ["Business"] = new List<string> { "business", "company", "market", "product", "service", "startup", "entrepreneur", "industry", "corporate", "trade" },
                ["Health"] = new List<string> { "health", "medical", "doctor", "hospital", "medicine", "wellness", "disease", "treatment", "fitness", "diet" },
                ["Science"] = new List<string> { "science", "research", "study", "discovery", "scientific", "physics", "biology", "chemistry", "space", "experiment" }
            };
            
            // Combine all text for matching
            var allText = $"{title} {description} {string.Join(" ", keywords)}".ToLower();
            
            var categoryScores = new Dictionary<string, int>();
            foreach (var category in categoryKeywords.Keys)
            {
                int score = 0;
                foreach (var keyword in categoryKeywords[category])
                {
                    if (allText.Contains(keyword))
                    {
                        score++;
                    }
                }
                categoryScores[category] = score;
            }
            
            // Get the category with highest score
            var topCategory = categoryScores.OrderByDescending(kv => kv.Value).First();
            
            // If score is too low, categorize as "Other"
            if (topCategory.Value < 2)
            {
                return "Other";
            }
            
            return topCategory.Key;
        }
        
        private bool IsWebsiteSafeForChildren(string title, string description, List<string> keywords)
        {
            // Simple check for inappropriate content
            var unsafeKeywords = new HashSet<string>
            {
                "adult", "mature", "explicit", "sex", "porn", "gambling", "violence", "drug", "alcohol", "tobacco",
                "weapon", "casino", "bet", "wager", "r-rated", "18+", "nsfw", "restricted"
            };
            
            // Combine all text for matching
            var allText = $"{title} {description} {string.Join(" ", keywords)}".ToLower();
            
            foreach (var keyword in unsafeKeywords)
            {
                if (allText.Contains(keyword))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        #endregion
    }
} 