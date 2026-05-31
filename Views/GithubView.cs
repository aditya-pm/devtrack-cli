using Spectre.Console;
using DevTrackCLI.Models;
using System.Runtime.CompilerServices;

namespace DevTrackCLI.Views;

static class GithubView
{
    public static Table ShowRepo(GithubRepo repo)
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

        return table;
    }

    public static BarChart ShowLanguages(Dictionary<string, double> languagePercentages)
    {
        var topLanguages = languagePercentages.OrderByDescending(x => x.Value).Take(5);
        // var table = new Table();

        // table.Border(TableBorder.DoubleEdge);
        // table.AddColumn("Language");
        // table.AddColumn("Percentage");
        // foreach (KeyValuePair<string, double> language in topLanguages)
        // {
        //     table.AddRow(language.Key, language.Value.ToString("F2"));
        // }

        // AnsiConsole.Write(table);

        var chart = new BarChart();
        foreach (KeyValuePair<string, double> language in topLanguages)
        {
            chart.AddItem(language.Key, Math.Round(language.Value, 2), Color.Orange1);
        }
        // AnsiConsole.Write(chart);
        return chart;
    }

    public static Table ShowCommits(List<GitHubCommit> commits)
    {
        var table = new Table();

        table.Border(TableBorder.Rounded);
        table.AddColumn("[yellow]Recent Commits:[/]");

        foreach (var commit in commits.Take(5))
        {
            string title = commit.Commit.Message.Split("\n")[0];
            table.AddRow(Markup.Escape("• " + title));
        }

        return table;
    }

    public static void ShowRecentRepos(List<string> recentRepos)
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

    public static void Show(GithubRepo repo,
                            Dictionary<string, double> languagePercentages,
                            List<GitHubCommit> commits)
    {
        Table repoTable = ShowRepo(repo);
        BarChart languagesChart = ShowLanguages(languagePercentages);

        Panel repoTablePanel = new Panel(repoTable).Header("[red] Stats [/]", Justify.Center).NoBorder();
        Panel languagesChartPanel = new Panel(new Panel(languagesChart)).Header("Languages").NoBorder();

        Grid repoAndLanguagesGrid = new Grid();
        repoAndLanguagesGrid.AddColumns(2);
        repoAndLanguagesGrid.AddRow(repoTablePanel, languagesChartPanel);
        repoAndLanguagesGrid.AddEmptyRow();

        AnsiConsole.Write(repoAndLanguagesGrid);

        AnsiConsole.Write(new Rule());
        AnsiConsole.WriteLine();

        Table recentCommits = ShowCommits(commits);
        AnsiConsole.Write(recentCommits);

    }
}