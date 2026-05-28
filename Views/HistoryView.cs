using Spectre.Console;
using DevTrackCLI.Services;

namespace DevTrackCLI.Views;

static class HistoryView
{
    public static async Task ShowRecentRepos()
    {
        HistoryService historyService = new();
        List<string> recentRepos = await historyService.GetRecentRepositoriesAsync();

        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Recent Searches[/]");

        foreach (string recentRepo in recentRepos)
        {
            table.AddRow(recentRepo);
        }

        AnsiConsole.Write(table);
    }
}