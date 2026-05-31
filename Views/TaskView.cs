using DevTrackCLI.Models;
using Spectre.Console;
namespace DevTrackCLI.Views;

static class TaskView
{
    public static void ShowTasks(List<TaskItem> tasks)
    {
        AnsiConsole.Clear();

        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("Title");
        table.AddColumn("Status");
        table.AddColumn("Created At");

        foreach (TaskItem task in tasks)
        {
            table.AddRow(
                task.Title,
                task.IsCompleted ? "Completed" : "Pending",
                task.CreatedAt.ToString()
            );
        }

        AnsiConsole.Write(table);
    }

    public static string DisplayChoices()
    {
        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("\nSelect an option:")
                .AddChoices(
                    "Add Task",
                    "Mark Task Completed",
                    "Delete Task",
                    "Exit"
                )
        );

        return choice;
    }
}