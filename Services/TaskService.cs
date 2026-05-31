using System.Text.Json;
using DevTrackCLI.Interfaces;
using DevTrackCLI.Models;

namespace DevTrackCLI.Services;

class TaskService : ITaskService
{
    static private readonly string filePath = "Data/tasks.json";

    public async Task AddTaskAsync(string taskName)
    {
        TaskItem newTask = new()
        {
            Title = taskName,
            IsCompleted = false,
            CreatedAt = DateTime.Now
        };

        List<TaskItem> tasks = await ListTasksAsync();
        tasks.Add(newTask);

        await SaveTasksAsync(tasks);
    }

    public async Task<List<TaskItem>> ListTasksAsync()
    {
        if (!File.Exists(filePath)) return new List<TaskItem>();

        string json = await File.ReadAllTextAsync(filePath);
        List<TaskItem>? tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
        return tasks ?? new List<TaskItem>();
    }

    public async Task CompleteTasksAsync(List<string> taskNames)
    {
        List<TaskItem> tasks = await ListTasksAsync();

        foreach (TaskItem task in tasks)
        {
            if (taskNames.Contains(task.Title)) task.IsCompleted = true;
        }

        await SaveTasksAsync(tasks);
    }

    public async Task DeleteTasksAsync(List<string> taskNames)
    {
        List<TaskItem> tasks = await ListTasksAsync();
        tasks.RemoveAll(task => taskNames.Contains(task.Title));

        await SaveTasksAsync(tasks);
    }

    private async Task SaveTasksAsync(List<TaskItem> tasks)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(tasks, options);
        await File.WriteAllTextAsync(filePath, json);
    }
}
