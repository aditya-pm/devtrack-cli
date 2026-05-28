using DevTrackCLI.Models;

namespace DevTrackCLI.Interfaces;

interface IGithubService
{
    Task<GithubRepo?> GetRepositoryAsync(string repoName);
}