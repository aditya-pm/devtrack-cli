using DevTrackCLI.Models;

namespace DevTrackCLI.Interfaces;

interface ITaskService
{
    Task AddTaskAsync(string taskName);
    Task<List<TaskItem>> ListTasksAsync();
    Task CompleteTaskAsync(string taskName);
    Task DeleteTask(string taskName);
}