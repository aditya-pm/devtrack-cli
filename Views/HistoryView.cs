using Spectre.Console;

namespace DevTrackCLI.Views;

static class HistoryView
{
    public static async Task ShowRecentRepos(List<string> recentRepos)
    {
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