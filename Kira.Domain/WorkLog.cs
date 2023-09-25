namespace Kira.Domain;

using System.Text.Json.Serialization;

public record WorkLog
{
    [JsonPropertyName("startAt")] public int StartAt { get; set; }
    [JsonPropertyName("maxResults")] public int MaxResults { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("worklogs")] public List<WorklogDetail> Worklogs { get; } = new();
}

public record WorklogDetail
{
    [JsonPropertyName("self")] public string Self { get; set; } = string.Empty;
    [JsonPropertyName("author")] public User Author { get; set; } = new();
    [JsonPropertyName("updateAuthor")] public User UpdateAuthor { get; set; } = new();
    [JsonPropertyName("comment")] public string Comment { get; set; } = string.Empty;
    [JsonPropertyName("created")] public string Created { get; set; } = string.Empty;
    [JsonPropertyName("updated")] public string Updated { get; set; } = string.Empty;
    [JsonPropertyName("started")] public string Started { get; set; } = string.Empty;
    [JsonPropertyName("timeSpent")] public object? TimeSpent { get; set; }
    [JsonPropertyName("timeSpentSeconds")] public int TimeSpentSeconds { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
    [JsonPropertyName("issueId")] public string IssueId { get; set; } = string.Empty;
}