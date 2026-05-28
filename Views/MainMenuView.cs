using Spectre.Console;

namespace DevTrackCLI.Views;

static class MainMenuView
{
    public static string Show()
    {
        AnsiConsole.Clear();

        Panel panel = new Panel("[bold green]DEVTRACK CLI[/]");
        panel.Border = BoxBorder.Double;
        AnsiConsole.Write(panel);

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\n[yellow]Select an option:[/]")
                .AddChoices(
                    "GitHub Repository Summary",
                    "Exit"
                )
        );

        return choice;
    }
}