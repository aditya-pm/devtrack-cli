using Spectre.Console;
using DevTrackCLI.Models;

namespace DevTrackCLI.Views;

static class GithubView
{
    public static void ShowRepo(GithubRepo repo)
    {
        var table = new Table();

        table.Border(TableBorder.Rounded);

        table.AddColumn("[yellow]Property[/]");
        table.AddColumn("[green]Value[/]");

        table.AddRow("Repository", repo.FullName);
        table.AddRow("Description", repo.Description);
        table.AddRow("Stars", repo.StargazersCount.ToString());
        table.AddRow("Forks", repo.ForksCount.ToString());
        table.AddRow("Open Issues", repo.OpenIssuesCount.ToString());
        table.AddRow("Last Updated", repo.UpdatedAt.ToString());

        AnsiConsole.Write(table);
    }
}