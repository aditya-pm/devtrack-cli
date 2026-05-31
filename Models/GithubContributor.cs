using System.Text.Json.Serialization;
namespace DevTrackCLI.Models;

public class GithubContributor
{
    [JsonPropertyName("login")]
    public string Login { get; set; } = "";

    [JsonPropertyName("contributions")]
    public int Contributions { get; set; }
}