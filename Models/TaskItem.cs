namespace DevTrackCLI.Models;

class TaskItem
{
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}