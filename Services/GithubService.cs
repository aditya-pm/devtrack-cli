using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using DevTrackCLI.Interfaces;
using DevTrackCLI.Models;

namespace DevTrackCLI.Services;

class GithubService : IGithubService
{
    private readonly HttpClient httpClient;

    public GithubService()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        string token = configuration["GitHub:Token"] ?? "";
        httpClient = new HttpClient();
        // GitHub required headers
        httpClient.DefaultRequestHeaders.Add(
            "User-Agent",
            "DevTrackCLI"
        );
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token
            );
    }

    public async Task<GithubRepo?> GetRepositoryAsync(string repoName)
    {
        try
        {
            string url = $"https://api.github.com/repos/{repoName}";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();

            GithubRepo? repo = JsonSerializer.Deserialize<GithubRepo>(json);
            return repo;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<List<GitHubCommit>?> GetRecentCommitsAsync(string repoName)
    {
        try
        {
            string url = $"https://api.github.com/repos/{repoName}/commits";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();

            List<GitHubCommit>? commits = JsonSerializer.Deserialize<List<GitHubCommit>>(json);

            return commits;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Dictionary<string, double>?> GetLanguagesAsync(string repoName)
    {
        try
        {
            string url = $"https://api.github.com/repos/{repoName}/languages";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();
            Dictionary<string, int>? languages = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
            if (languages == null) return null;

            Dictionary<string, double> languagePercentages = [];
            int totalBytes = languages.Values.Sum();
            foreach (KeyValuePair<string, int> language in languages)
            {
                double percentage = (double)language.Value / totalBytes * 100;
                languagePercentages.Add(language.Key, percentage);
            }

            return languagePercentages;

        }
        catch
        {
            return null;
        }
    }

    public async Task<List<GithubContributor>?> GetContributorsAsync(string repoName)
    {
        try
        {
            string url = $"https://api.github.com/repos/{repoName}/contributors";

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<GithubContributor>>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}
