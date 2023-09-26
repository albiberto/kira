namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Fields
{
    [JsonPropertyName("summary")] public string Summary { get; set; } = null!;

    [JsonPropertyName("components")] public List<Component> Components { get; set; } = new();

    [JsonPropertyName("issuetype")] public Type Type { get; set; } = new();
    
    [JsonPropertyName("reporter")] public User? Reporter { get; set; } = new();

    [JsonPropertyName("assignee")] public User? Assignee { get; set; } = new();

    [JsonPropertyName("priority")] public Priority Priority { get; set; } = new();

    [JsonPropertyName("labels")] public List<string> Labels { get; set; } = new();

    [JsonPropertyName("status")] public Status Status { get; set; } = new();
    
    [JsonPropertyName("parent")] public Parent Parent { get; set; } = new();
    
    [JsonPropertyName("progress")] public IssueProgress Progress { get; set; } = new();
    
    
    
    [JsonPropertyName("timeoriginalestimate")] public int? TimeOriginalEstimate { get; set; }
    
    [JsonPropertyName("timeestimate")] public int? TimeEstimate { get; set; }

    [JsonPropertyName("timespent")] public int? TimeSpent { get; set; }
}