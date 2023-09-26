namespace Kira.Domain;

using System.Text.Json.Serialization;

public class Status
{
    [JsonPropertyName("self")] public string Self { get; set; } = null!;

    [JsonPropertyName("description")] public string Description { get; set; } = null!;

    [JsonPropertyName("iconUrl")] public string IconUrl { get; set; } = null!;

    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("id")] public string Id { get; set; } = null!;

    [JsonPropertyName("statusCategory")] public StatusCategory StatusCategory { get; set; } = new();
}

public class StatusCategory
{
    [JsonPropertyName("self")] public string Self { get; set; } = null!;

    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("key")] public string Key { get; set; } = null!;

    [JsonPropertyName("colorName")] public string ColorName { get; set; } = null!;

    [JsonPropertyName("name")] public string Name { get; set; } = null!;
}