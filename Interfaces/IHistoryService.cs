namespace DevTrackCLI.Interfaces;

interface IHistoryService
{
    Task SaveRepositoryAsync(string repoName);
    Task<List<string>> GetRecentRepositoriesAsync();
}
