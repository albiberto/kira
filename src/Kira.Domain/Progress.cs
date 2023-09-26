namespace Kira.Domain;

using System.Text.Json.Serialization;

public record IssueProgress
{
    [JsonPropertyName("progress")] public int? Progress { get; set; }

    [JsonPropertyName("total")] public int? Total { get; set; }

    [JsonPropertyName("percent")] public int? Percent { get; set; }
}