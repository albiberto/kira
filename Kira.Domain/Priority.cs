namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Priority
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("self")] public string Self { get; set; } = string.Empty;
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("iconUrl")] public string IconUrl { get; set; } = string.Empty;
}