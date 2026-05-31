using Spectre.Console;
using DevTrackCLI.Models;
using Spectre.Console.Rendering;

namespace DevTrackCLI.Views;

static class GithubView
{
    private static Table GetRepoTable(GithubRepo repo)
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

    private static BarChart GetLanguagesChart(Dictionary<string, double> languagePercentages)
    {
        var topLanguages = languagePercentages.OrderByDescending(x => x.Value).Take(5);

        var chart = new BarChart();
        foreach (KeyValuePair<string, double> language in topLanguages)
        {
            chart.AddItem(language.Key, Math.Round(language.Value, 2), Color.Orange1);
        }
        // AnsiConsole.Write(chart);
        return chart;
    }

    private static Panel GetCommitsPanel(List<GitHubCommit> commits)
    {
        var commitsList = new List<IRenderable>();
        commitsList.Add(new Rule() { Style = Style.Parse("grey") });

        foreach (var commit in commits.Take(5))
        {
            string title = commit.Commit.Message.Split("\n")[0];
            if (title.Length > 50)
            {
                title = title[..47] + "...";
            }
            commitsList.Add(new Text(Markup.Escape("• " + title)));
            commitsList.Add(new Rule() { Style = Style.Parse("grey") });
        }

        var rows = new Rows(commitsList);

        return new Panel(rows);
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

    private static BarChart GetContributorsChart(List<GithubContributor> contributors)
    {
        double totalContributions = contributors
                                        .Take(5)
                                        .Sum(contributor => contributor.Contributions);
        List<double> contributorsPercentages = contributors
                                                .Take(5)
                                                .Select(contributor => contributor.Contributions / totalContributions * 100).ToList();
    
        var chart = new BarChart();
        for (int i = 0; i < contributorsPercentages.Count; i++)
        {
            chart.AddItem(contributors[i].Login, Math.Round(contributorsPercentages[i], 2));
        }
        return chart;
    }

    public static void Show(GithubRepo repo,
                            Dictionary<string, double> languagePercentages,
                            List<GitHubCommit> commits,
                            List<GithubContributor> contributors)
    {
        Table repoTable = GetRepoTable(repo);
        BarChart languagesChart = GetLanguagesChart(languagePercentages);
        Panel recentCommits = GetCommitsPanel(commits);
        BarChart contributorsTable = GetContributorsChart(contributors);

        Panel repoTablePanel = new Panel(repoTable)
                                    .Header("[red]Stats[/]", Justify.Center).NoBorder();
        Panel languagesChartPanel = new Panel(new Panel(languagesChart))
                                    .Header("[red]Top 5 Languages[/]", Justify.Center).NoBorder();
        Panel recentCommitsPanel = new Panel(recentCommits)
                                    .Header("[red]Recent Commits[/]", Justify.Center).NoBorder();
        Panel contributorsTablelPanel = new Panel(new Panel(contributorsTable))
                                    .Header("[red]Top 5 Contributors[/]", Justify.Center).NoBorder();

        var githubViewGrid = new Grid();
        githubViewGrid.AddColumns(2);
        githubViewGrid.AddRow(repoTablePanel, languagesChartPanel);
        githubViewGrid.AddEmptyRow();
        githubViewGrid.AddRow(recentCommitsPanel, contributorsTablelPanel);
        githubViewGrid.AddEmptyRow();

        AnsiConsole.Write(githubViewGrid);
    }
}