namespace Kira.Domain;

using System.Text.Json.Serialization;

public class Component
{
    [JsonPropertyName("self")] public string Self { get; set; } = null!;
    [JsonPropertyName("id")] public string Id { get; set; } = null!;
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("lead")] public User Lead { get; set; } = null!;
    [JsonPropertyName("assigneeType")] public string AssigneeType { get; set; } = null!;
    [JsonPropertyName("assignee")] public User Assignee { get; set; } = null!;
    [JsonPropertyName("realAssigneeType")] public string RealAssigneeType { get; set; } = null!;
    [JsonPropertyName("realAssignee")] public User RealAssignee { get; set; } = null!;
    [JsonPropertyName("isAssigneeTypeValid")] public bool IsAssigneeTypeValid { get; set; }
    [JsonPropertyName("project")] public string Project { get; set; } = null!;
    [JsonPropertyName("projectId")] public int ProjectId { get; set; }
}