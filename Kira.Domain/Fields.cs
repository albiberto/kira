namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Fields
{
    [JsonPropertyName("summary")] public string Summary { get; set; } = string.Empty;
    [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;

    [JsonPropertyName("parent")] public Issue? Parent { get; set; }
    [JsonPropertyName("status")] public Status Status { get; set; } = new();
    [JsonPropertyName("priority")] public Priority Priority { get; set; } = new();
    [JsonPropertyName("issuetype")] public Type Type { get; set; } = new();
    [JsonPropertyName("subtasks")] public IEnumerable<Issue> Subtasks { get; set; } = Enumerable.Empty<Issue>();

    [JsonPropertyName("components")] public IEnumerable<Component> Components { get; set; } = Enumerable.Empty<Component>();
    [JsonPropertyName("labels")] public IEnumerable<object> Labels { get; set; } = Array.Empty<object>();

    [JsonPropertyName("assignee")] public User Assignee { get; set; } = new();
    [JsonPropertyName("reporter")] public User Reporter { get; set; } = new();

    [JsonPropertyName("project")] public Project Project { get; set; } = new();

    [JsonPropertyName("attachment")] public IEnumerable<object> Attachment { get; } = Array.Empty<object>();
    [JsonPropertyName("versions")] public IEnumerable<object> Versions { get; } = Array.Empty<object>();
    [JsonPropertyName("fixVersions")] public IEnumerable<object> FixVersions { get; } = Array.Empty<object>();

    [JsonPropertyName("duedate")] public object? DueDate { get; set; }
    [JsonPropertyName("resolution")] public object? Resolution { get; set; }
    [JsonPropertyName("resolutiondate")] public object? ResolutionDate { get; set; }
    [JsonPropertyName("comment")] public Comment Comment { get; set; } = new();
    [JsonPropertyName("worklog")] public WorkLog WorkLog { get; set; } = new();
    [JsonPropertyName("aggregateprogress")] public Progress AggregateProgress { get; set; } = new();
    [JsonPropertyName("progress")] public Progress IssueProgress { get; set; } = new();
    [JsonPropertyName("aggregatetimeoriginalestimate")] public object? AggregateTimeOriginalEstimate { get; set; }
    [JsonPropertyName("timeestimate")] public object? OriginalEstimate { get; set; }
    [JsonPropertyName("aggregatetimespent")] public object? AggregateTimeSpent { get; set; }
    [JsonPropertyName("timespent")] public object? TimeSpent { get; set; }
    [JsonPropertyName("aggregatetimeestimate")] public object? AggregateTimeEstimate { get; set; }
    [JsonPropertyName("timeoriginalestimate")] public object? RemainingEstimate { get; set; }
    [JsonPropertyName("timetracking")] public TimeTracking TimeTracking { get; set; } = new();
    [JsonPropertyName("votes")] public Vote Vote { get; set; } = new();
    [JsonPropertyName("watches")] public Watches Watches { get; set; } = new();
    [JsonPropertyName("archivedby")] public object? ArchivedBy { get; set; }
    [JsonPropertyName("archiveddate")] public object? ArchivedDate { get; set; }
    [JsonPropertyName("creator")] public User Creator { get; set; } = new();
    [JsonPropertyName("created")] public string Created { get; set; } = string.Empty;
    [JsonPropertyName("updated")] public string Updated { get; set; } = string.Empty;
    [JsonPropertyName("lastViewed")] public string LastViewed { get; set; } = string.Empty;
    [JsonPropertyName("environment")] public object? Environment { get; set; }
    [JsonPropertyName("workratio")] public int WorkRatio { get; set; }
}