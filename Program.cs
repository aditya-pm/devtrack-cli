using DevTrackCLI.Models;
using DevTrackCLI.Services;
using DevTrackCLI.Views;
using Spectre.Console;

class Program
{
    static async Task Main(string[] args)
    {
        GithubService githubService = new();
        HistoryService historyService = new();

        bool running = true;

        while (running)
        {
            string choice = MainMenuView.Show();

            switch(choice)
            {
                case "GitHub Repository Summary":
                    AnsiConsole.MarkupLine("[green]Recent Repositories:[/]");
                    List<string> recentRepos = await  historyService.GetRecentRepositoriesAsync();
                    foreach (string recentRepo in recentRepos)
                    {
                        AnsiConsole.MarkupLine($"[grey]{recentRepo}[/]");
                    }
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

                    AnsiConsole.Clear();

                    if (repo == null)
                    {
                        AnsiConsole.MarkupLine("[red]Repository not found.[/]");
                    }
                    else
                    {
                        GithubView.ShowRepo(repo);
                        await historyService.SaveRepositoryAsync(repoName);
                    }

                    AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
                    Console.ReadKey();
                    break;

                case "Exit":
                    running = false;
                    break;
            }
        }
    }
}
