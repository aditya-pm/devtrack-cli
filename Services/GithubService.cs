using System.Text.Json;
using DevTrackCLI.Interfaces;
using DevTrackCLI.Models;

namespace DevTrackCLI.Services;

class GithubService : IGithubService
{
    private readonly HttpClient httpClient;

    public GithubService()
    {
        httpClient = new HttpClient();
        // GitHub required headers
        httpClient.DefaultRequestHeaders.Add(
            "User-Agent",
            "DevTrackCLI"
        );
    }

    public async Task<GithubRepo?> GetRepositoryAsync(string repoName)
    {
        try
        {
            string url = $"https://api.github.com/repos/{repoName}";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            string json = await response.Content.ReadAsStringAsync();

            GithubRepo? repo = JsonSerializer.Deserialize<GithubRepo>(json);
            return repo;
        }
        catch
        {
            return null;
        }
    }
}
