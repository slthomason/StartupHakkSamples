using ModelContextProtocol.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCPServer.CSharp;

[McpServerToolType]
public static class WebSurferTool
{
    private static readonly HttpClient _httpClient = new HttpClient();
    
    [McpServerTool, Description("Fetches and returns the content of a web page.")]
    public static async Task<string> WebPageContent(string url)
    {
        try
        {
            url = NormalizeUrl(url);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return $"Error fetching web page: {ex.Message}";
        }
    }

    [McpServerTool, Description("Extracts and returns the text content from a web page.")]
    public static async Task<string> WebPageText(string url)
    {
        try
        {
            string html = await WebPageContent(url);
            // Simple HTML tag removal - a more robust solution would use HtmlAgilityPack
            string text = Regex.Replace(html, "<[^>]*>", string.Empty);
            text = Regex.Replace(text, @"\s+", " ").Trim();
            return text;
        }
        catch (Exception ex)
        {
            return $"Error extracting text: {ex.Message}";
        }
    }

    [McpServerTool, Description("Extracts links from a web page.")]
    public static async Task<List<string>> WebPageLinks(string url, int maxLinks = 20)
    {
        try
        {
            string baseUrl = NormalizeUrl(url);
            string html = await WebPageContent(url);
            
            var matches = Regex.Matches(html, @"<a\s+(?:[^>]*?\s+)?href=""([^""]*)""", RegexOptions.IgnoreCase);
            
            var links = new List<string>();
            foreach (Match match in matches)
            {
                if (links.Count >= maxLinks) break;
                
                string href = match.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(href) || href.StartsWith("#") || href.StartsWith("javascript:"))
                    continue;
                
                // Resolve relative URLs
                if (href.StartsWith("/"))
                {
                    Uri baseUri = new Uri(baseUrl);
                    href = $"{baseUri.Scheme}://{baseUri.Host}{href}";
                }
                else if (!href.StartsWith("http"))
                {
                    if (!baseUrl.EndsWith("/"))
                        baseUrl += "/";
                    href = baseUrl + href;
                }
                
                links.Add(href);
            }
            
            return links;
        }
        catch (Exception ex)
        {
            return new List<string> { $"Error extracting links: {ex.Message}" };
        }
    }

    [McpServerTool, Description("Gets the title of a web page.")]
    public static async Task<string> WebPageTitle(string url)
    {
        try
        {
            url = NormalizeUrl(url);
            string html = await WebPageContent(url);
            
            Match match = Regex.Match(html, @"<title>\s*(.*?)\s*</title>", RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value;
            else
                return "No title found";
        }
        catch (Exception ex)
        {
            return $"Error getting title: {ex.Message}";
        }
    }

    [McpServerTool, Description("Performs a simple search using Bing and returns the top results.")]
    public static async Task<List<string>> WebSearch(string query, int maxResults = 5)
    {
        try
        {
            // Note: In a real implementation, you would use a proper API
            // This is a simplified example that scrapes results
            string url = $"https://www.bing.com/search?q={Uri.EscapeDataString(query)}";
            string html = await WebPageContent(url);
            
            var results = new List<string>();
            var matches = Regex.Matches(html, @"<h2><a href=""([^""]+)""[^>]*>(.*?)</a></h2>");
            
            foreach (Match match in matches)
            {
                if (results.Count >= maxResults) break;
                
                string link = match.Groups[1].Value;
                string title = Regex.Replace(match.Groups[2].Value, "<[^>]*>", string.Empty);
                
                results.Add($"{title} - {link}");
            }
            
            return results.Count > 0 ? results : new List<string> { "No search results found" };
        }
        catch (Exception ex)
        {
            return new List<string> { $"Error performing search: {ex.Message}" };
        }
    }

    [McpServerTool, Description("Summarizes the content of a web page.")]
    public static async Task<string> SummarizeWebPage(string url, int maxSentences = 5)
    {
        try
        {
            string text = await WebPageText(url);
            
            // Simple sentence splitting - a more robust solution would handle edge cases
            var sentences = Regex.Split(text, @"(?<=[.!?])\s+")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Take(maxSentences)
                .ToList();
            
            return string.Join(" ", sentences);
        }
        catch (Exception ex)
        {
            return $"Error summarizing web page: {ex.Message}";
        }
    }

    // Helper method to ensure URLs are properly formatted
    private static string NormalizeUrl(string url)
    {
        if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            url = "https://" + url;
        
        return url;
    }
} 