using System.Text.Json;
using DevTrackCLI.Interfaces;

namespace DevTrackCLI.Services;

class HistoryService : IHistoryService
{
    static private readonly string filePath = "Data/recent_repos.json";

    public async Task SaveRepositoryAsync(string repoName)
    {
        List<string> repos = await GetRecentRepositoriesAsync();

        if (!repos.Contains(repoName))
            repos.Add(repoName);

        var options = new JsonSerializerOptions { WriteIndented = true };

        string json = JsonSerializer.Serialize(repos, options);

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<List<string>> GetRecentRepositoriesAsync()
    {
        if (!File.Exists(filePath)) return new List<string>();

        string json = await File.ReadAllTextAsync(filePath);
        List<string>? repos = JsonSerializer.Deserialize<List<string>>(json);
        return repos ?? new List<string>();
    }
}

