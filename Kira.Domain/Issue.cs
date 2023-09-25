namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Issue
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;
    [JsonPropertyName("self")] public string Self { get; set; } = string.Empty;
    [JsonPropertyName("fields")] public Fields Fields { get; set; } = new();

    public record Current : Issue
    {
        [JsonPropertyName("expand")] public string? Expand { get; set; }
    }
}