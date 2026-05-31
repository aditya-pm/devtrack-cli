using Spectre.Console;

namespace DevTrackCLI.Views;

static class MainMenuView
{
    public static string Show()
    {
        AnsiConsole.Clear();

        var titleText = new FigletText("DevTrack CLI")
        {
            Color = Color.DarkSeaGreen,
            Justification = Justify.Center
        };

        var rows = new Rows(
            titleText,
            Align.Center(new Markup("[green]A Developer Productivity Tool[/]"))
        );

        Panel panel = new Panel(rows);
        panel.Border = BoxBorder.Double;

        var rootGrid = new Grid();
        rootGrid.AddColumns(2);

        var githubGrid = new Grid();
        githubGrid.AddColumn();
        githubGrid.AddRow("GitHub Analytics");
        githubGrid.AddRow(new Rule());
        githubGrid.AddRow("• Repo Name");
        githubGrid.AddRow("• Open Issues");
        githubGrid.AddRow("• Recent Commits");
        githubGrid.AddRow("• Languages");
        githubGrid.AddRow("• Fork count");

        var githubPanel = new Panel(githubGrid).NoBorder();

        var taskGrid = new Grid();
        taskGrid.AddColumn();
        taskGrid.AddRow("Task Manager");
        taskGrid.AddRow(new Rule());
        taskGrid.AddRow("• Add task");
        taskGrid.AddRow("• Complete task");
        taskGrid.AddRow("• Delete task");

        var taskPanel = new Panel(taskGrid).NoBorder();

        rootGrid.AddEmptyRow();
        rootGrid.AddRow(githubPanel, taskPanel);
        rootGrid.AddEmptyRow();

        AnsiConsole.Write(panel);
        AnsiConsole.Write(rootGrid);

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[yellow]Select an option:[/]")
                .AddChoices(
                    "GitHub Repository Summary",
                    "Task Management",
                    "Exit"
                )
        );

        return choice;
    }
}