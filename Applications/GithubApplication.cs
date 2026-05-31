using DevTrackCLI.Services;
using DevTrackCLI.Views;
using DevTrackCLI.Models;
using Spectre.Console;

namespace DevTrackCLI.Applications;

class GithubApplication
{
    private readonly GithubService githubService;
    private readonly HistoryService historyService;

    public GithubApplication()
    {
        githubService = new GithubService();
        historyService = new HistoryService();
    }

    public async Task RunAsync()
    {
        AnsiConsole.Clear();

        List<string> recentRepos = await historyService.GetRecentRepositoriesAsync();
        GithubView.ShowRecentRepos(recentRepos);

        AnsiConsole.WriteLine();

        string repoName = AnsiConsole.Ask<string>("\nEnter repository name:");

        // GithubRepo? repo = await githubService.GetRepositoryAsync(repoName);
        GithubRepo? repo = null;
        await AnsiConsole.Status().Spinner(Spinner.Known.Arc)
            .StartAsync(
                "[yellow]Fetching repository data...[/]",
                async ctx =>
                {
                    repo = await githubService.GetRepositoryAsync(repoName);
                }
            );

        Dictionary<string, double>? languages = await githubService.GetLanguagesAsync(repoName);
        List<GitHubCommit>? commits = await githubService.GetRecentCommitsAsync(repoName);
        List<GithubContributor>? contributors = await githubService.GetContributorsAsync(repoName);

        AnsiConsole.Clear();

        if (repo != null && languages != null && commits != null && contributors != null)
        {
            GithubView.Show(repo, languages, commits, contributors);
            await historyService.SaveRepositoryAsync(repoName);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Repository not found.[/]");
        }

        AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
        Console.ReadKey();
    }
}