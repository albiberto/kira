namespace Kira.Domain;

using System.Text.Json.Serialization;

public record TimeTracking
{
    [JsonPropertyName("timeSpent")] public object? TimeSpent { get; set; }
    [JsonPropertyName("timeSpentSeconds")] public int TimeSpentSeconds { get; set; }
}