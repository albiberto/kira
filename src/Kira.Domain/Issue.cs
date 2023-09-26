namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Issue
{
    [JsonPropertyName("expand")] public string Expand { get; set; } = null!;

    [JsonPropertyName("id")] public string Id { get; set; } = null!;

    [JsonPropertyName("self")] public string Self { get; set; } = null!;

    [JsonPropertyName("key")] public string Key { get; set; } = null!;

    [JsonPropertyName("fields")] public Fields Fields { get; set; } = new();
}