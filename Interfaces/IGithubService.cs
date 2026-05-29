using DevTrackCLI.Models;

namespace DevTrackCLI.Interfaces;

interface IGithubService
{
    Task<GithubRepo?> GetRepositoryAsync(string repoName);
    Task<List<GitHubCommit>?> GetRecentCommitsAsync(string repoName);
}