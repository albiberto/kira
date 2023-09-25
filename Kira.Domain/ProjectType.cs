namespace Kira.Domain;

using System.Text.Json.Serialization;

public class ProjectType
{
    [JsonPropertyName("self")] public string Self { get; set; } = null!;

    [JsonPropertyName("id")] public string Id { get; set; } = null!;

    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("subtask")] public bool Subtask { get; set; }

    [JsonPropertyName("statuses")] public List<Status> Statuses { get; set; } = new();
}