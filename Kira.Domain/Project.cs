namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Project
{
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;
    [JsonPropertyName("self")] public string Self { get; set; } = string.Empty;
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("projectTypeKey")] public string ProjectTypeKey { get; set; } = string.Empty;
    [JsonPropertyName("avatarUrls")] public AvatarUrls AvatarUrls { get; set; } = new();
}