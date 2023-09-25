namespace Kira.Domain;

using System.Text.Json.Serialization;

public record JiraIssues
{
    [JsonPropertyName("expand")] public string Expand { get; set; } = string.Empty;
    [JsonPropertyName("startAt")] public int StartAt { get; set; }
    [JsonPropertyName("maxResults")] public int MaxResults { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("issues")] public IEnumerable<Issue> Issues { get; set; } = Enumerable.Empty<Issue>();
}