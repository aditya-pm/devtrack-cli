using Spectre.Console;
using DevTrackCLI.Models;

namespace DevTrackCLI.Views;

static class CommitView
{
    public static void ShowCommits(List<GitHubCommit> commits)
    {
        var table = new Table();

        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Recent Commits:[/]");

        foreach (var commit in commits.Take(5))
        {
            table.AddRow(commit.Commit.Message);
        }

        AnsiConsole.Write(table);
    }
}
