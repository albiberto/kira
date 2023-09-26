namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Parent
{
    [JsonPropertyName("id")] public string Id { get; set; } = null!;

    [JsonPropertyName("key")] public string Key { get; set; } = null!;

    [JsonPropertyName("self")] public string Self { get; set; } = null!;

    [JsonPropertyName("fields")] public ParentFields Fields { get; set; } = new();
}

public record ParentFields
{
    [JsonPropertyName("summary")] public string Summary { get; set; } = null!;

    [JsonPropertyName("status")] public Status Status { get; set; } = new();

    [JsonPropertyName("priority")] public Priority Priority { get; set; } = new();

    [JsonPropertyName("issuetype")] public Type Type { get; set; } = new();
}