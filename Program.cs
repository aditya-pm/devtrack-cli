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
                    AnsiConsole.Clear();

                    List<string> recentRepos = await historyService.GetRecentRepositoriesAsync();
                    GithubView.ShowRecentRepos(recentRepos);
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

                    Dictionary<string, double>? languages = await githubService.GetLanguagesAsync(repoName);
                    List<GitHubCommit>? commits = await githubService.GetRecentCommitsAsync(repoName);
                    List<GithubContributor>? contributors = await githubService.GetContributorsAsync(repoName);

                    AnsiConsole.Clear();

                    if (repo != null && languages != null && commits != null && contributors != null)
                    {
                        GithubView.Show(repo, languages, commits, contributors);
                        await historyService.SaveRepositoryAsync(repoName);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Repository not found.[/]");
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
