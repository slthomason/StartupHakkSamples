using System.ComponentModel;
using System.Text;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace MCPServer.CSharp;

[McpServerToolType]
public static class FileSystemTools
{
    [McpServerTool, Description("Lists files and directories in the specified path.")]
    public static string ListDirectory(
        [Description("The directory path to list (default: current directory)")] string path = ".")
    {
        try
        {
            var directoryInfo = new DirectoryInfo(path);
            
            if (!directoryInfo.Exists)
                return $"Directory not found: {path}";
                
            var directories = directoryInfo.GetDirectories()
                .Select(d => new FileSystemItem
                {
                    Name = d.Name,
                    Path = d.FullName,
                    Type = "Directory",
                    LastModified = d.LastWriteTime
                });
                
            var files = directoryInfo.GetFiles()
                .Select(f => new FileSystemItem
                {
                    Name = f.Name,
                    Path = f.FullName,
                    Type = "File",
                    Size = f.Length,
                    LastModified = f.LastWriteTime
                });
                
            var items = directories.Concat(files).ToList();
            
            return JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error listing directory: {ex.Message}";
        }
    }

    [McpServerTool, Description("Reads the content of a text file.")]
    public static string ReadFile(
        [Description("The path to the file to read")] string path)
    {
        try
        {
            if (!File.Exists(path))
                return $"File not found: {path}";
                
            var content = File.ReadAllText(path);
            return content;
        }
        catch (Exception ex)
        {
            return $"Error reading file: {ex.Message}";
        }
    }

    [McpServerTool, Description("Writes content to a text file.")]
    public static string WriteFile(
        [Description("The path to the file to write")] string path,
        [Description("The content to write to the file")] string content,
        [Description("Whether to append to the file instead of overwriting it (default: false)")] bool append = false)
    {
        try
        {
            if (append)
                File.AppendAllText(path, content);
            else
                File.WriteAllText(path, content);
                
            return $"Successfully wrote to file: {path}";
        }
        catch (Exception ex)
        {
            return $"Error writing to file: {ex.Message}";
        }
    }

    [McpServerTool, Description("Searches for files containing specific text.")]
    public static string SearchFiles(
        [Description("The directory to search in")] string directory,
        [Description("The text to search for")] string searchText,
        [Description("File pattern to match (e.g., *.txt, *.cs) (default: *.*)")] string pattern = "*.*",
        [Description("Whether to search in subdirectories (default: true)")] bool recursive = true)
    {
        try
        {
            var results = new List<FileSearchResult>();
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            
            foreach (var file in Directory.GetFiles(directory, pattern, searchOption))
            {
                try
                {
                    var content = File.ReadAllText(file);
                    if (content.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        var lineMatches = new List<LineMatch>();
                        var lines = File.ReadAllLines(file);
                        
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].Contains(searchText, StringComparison.OrdinalIgnoreCase))
                            {
                                lineMatches.Add(new LineMatch
                                {
                                    LineNumber = i + 1,
                                    LineContent = lines[i]
                                });
                            }
                        }
                        
                        results.Add(new FileSearchResult
                        {
                            FilePath = file,
                            LineMatches = lineMatches
                        });
                    }
                }
                catch
                {
                    // Skip files that can't be read
                }
            }
            
            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error searching files: {ex.Message}";
        }
    }
}

public class FileSystemItem
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
}

public class FileSearchResult
{
    public string FilePath { get; set; } = string.Empty;
    public List<LineMatch> LineMatches { get; set; } = new List<LineMatch>();
}

public class LineMatch
{
    public int LineNumber { get; set; }
    public string LineContent { get; set; } = string.Empty;
} 