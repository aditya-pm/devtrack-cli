using DevTrackCLI.Models;
using Spectre.Console;
namespace DevTrackCLI.Views;

static class TaskView
{
    public static void ShowTasks(List<TaskItem> tasks)
    {
        AnsiConsole.Clear();

        if (tasks.Count == 0)
        {
            Panel noTasks = new Panel("No tasks added!");
            AnsiConsole.Write(noTasks);
        }
        else
        {
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

            var taskPanel = new Panel(table).Header("Tasks Overview", Justify.Center)
                .Padding(1, 1)
                .BorderColor(Color.Aqua)
                .RoundedBorder();

            AnsiConsole.Write(taskPanel);
        }
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