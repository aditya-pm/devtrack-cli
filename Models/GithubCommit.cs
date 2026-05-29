using System.Text.Json.Serialization;

namespace DevTrackCLI.Models;

class GitHubCommits
{
    [JsonPropertyName("commit")]
    public CommitInfo Commit { get; set; } = new();
}

class CommitInfo
{
    [JsonPropertyName("message")]
    public string Message { get; set;} = "";
}
