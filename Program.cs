using DevTrackCLI.Models;
using DevTrackCLI.Services;
using DevTrackCLI.Views;
using Spectre.Console;

class Program
{
    static async Task Main(string[] args)
    {
        GithubService githubService = new();
        HistoryService historyService = new();
        TaskService taskService = new();

        bool running = true;

        while (running)
        {
            string choice = MainMenuView.Show();

            switch (choice)
            {
                case "GitHub Repository Summary":
                    List<string> recentRepos = await historyService.GetRecentRepositoriesAsync();
                    await HistoryView.ShowRecentRepos(recentRepos);
                    AnsiConsole.WriteLine();

                    string repoName = AnsiConsole.Ask<string>("\nEnter repository name:");

                    // GithubRepo? repo = await githubService.GetRepositoryAsync(repoName);
                    GithubRepo? repo = null;
                    await AnsiConsole.Status().Spinner(Spinner.Known.Arc)
                        .StartAsync(
                            "[yellow]Fetching repository data...[/]",
                            async ctx =>
                            {
                                repo = await githubService.GetRepositoryAsync(repoName);
                            }
                        );

                    AnsiConsole.Clear();

                    if (repo == null)
                    {
                        AnsiConsole.MarkupLine("[red]Repository not found.[/]");
                    }
                    else
                    {
                        GithubView.ShowRepo(repo);
                        await historyService.SaveRepositoryAsync(repoName);
                    }

                    Dictionary<string, double>? languages = await githubService.GetLanguagesAsync(repoName);
                    if (languages != null)
                    {
                        LanguageView.ShowLanguages(languages);
                    }

                    var commits = await githubService.GetRecentCommitsAsync(repoName);
                    if (commits != null)
                    {
                        CommitView.ShowCommits(commits);
                    }

                    AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
                    Console.ReadKey();
                    break;

                case "Task Management":
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
                    break;

                case "Exit":
                    running = false;
                    break;
            }
        }
    }
}
