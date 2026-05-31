using DevTrackCLI.Models;

namespace DevTrackCLI.Interfaces;

interface ITaskService
{
    Task AddTaskAsync(string taskName);
    Task<List<TaskItem>> ListTasksAsync();
    Task CompleteTasksAsync(List<string> taskNames);
    Task DeleteTasksAsync(List<string> taskNames);
}