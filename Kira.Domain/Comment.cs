namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Comment
{
    [JsonPropertyName("comments")] public List<object> Comments { get; } = new();
    [JsonPropertyName("maxResults")] public int MaxResults { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("startAt")] public int StartAt { get; set; }
}