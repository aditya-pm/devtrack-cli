using DevTrackCLI.Views;
using DevTrackCLI.Services;
using DevTrackCLI.Models;
using Spectre.Console;

namespace DevTrackCLI.Applications;

class TaskApplication
{
    private readonly TaskService taskService;

    public TaskApplication()
    {
        taskService = new TaskService();
    }

    public async Task RunAsync()
    {
        string taskChoice = "";
        while (taskChoice != "Exit")
        {
            AnsiConsole.Clear();

            List<TaskItem> tasks = await taskService.ListTasksAsync();

            List<string> taskNames = [];
            foreach (TaskItem task in tasks)
                taskNames.Add(task.Title);

            TaskView.ShowTasks(tasks);
            taskChoice = TaskView.DisplayChoices();

            if (taskChoice.Equals("Exit")) break;

            switch (taskChoice)
            {
                case "Add Task":
                    var taskName = AnsiConsole.Ask<string>("Enter task name: ");
                    await taskService.AddTaskAsync(taskName);
                    AnsiConsole.MarkupLine($"[green]Task {taskName} successfully added![/]");
                    break;

                case "Mark Task Completed":
                    if (taskNames.Count() == 0)
                    {
                        AnsiConsole.MarkupLine($"[red]No tasks added![/]");
                        break;
                    }
                    var selectedTasksToComplete = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                            .Title("Select tasks:")
                            .AddChoices(taskNames)
                    );
                    await taskService.CompleteTasksAsync(selectedTasksToComplete);
                    break;

                case "Delete Task":
                    if (taskNames.Count() == 0)
                    {
                        AnsiConsole.MarkupLine($"[red]No tasks added![/]");
                        break;
                    }
                    var selectedTasksToDelete = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                            .Title("Select tasks:")
                            .AddChoices(taskNames)
                    );
                    await taskService.DeleteTasksAsync(selectedTasksToDelete);
                    break;
            }

            AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
            Console.ReadKey();
        }
    }
}