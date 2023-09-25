namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Watches
{
    [JsonPropertyName("self")] public string Self { get; set; } = string.Empty;
    [JsonPropertyName("watchCount")] public int WatchCount { get; set; }
    [JsonPropertyName("isWatching")] public bool IsWatching { get; set; }
}