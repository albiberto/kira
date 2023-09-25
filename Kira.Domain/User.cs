namespace Kira.Domain;

using System.Text.Json.Serialization;

public record User
{
    [JsonConstructor]
    public User()
    {
    }

    [JsonPropertyName("self")] public string Self { get; set; } = "Unassigned";
    [JsonPropertyName("name")] public string Name { get; set; } = "Unassigned";
    [JsonPropertyName("key")] public string Key { get; set; } = "Unassigned";
    [JsonPropertyName("emailAddress")] public string EmailAddress { get; set; } = "Unassigned";
    [JsonPropertyName("avatarUrls")] public AvatarUrls AvatarUrls { get; set; } = new();
    [JsonPropertyName("displayName")] public string DisplayName { get; set; } = "Unassigned";
    [JsonPropertyName("active")] public bool Active { get; set; }
    [JsonPropertyName("timeZone")] public string? TimeZone { get; set; }
}