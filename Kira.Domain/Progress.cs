namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Progress
{
    [JsonPropertyName("progress")] public int AggregateProgress { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("percent")] public int Percent { get; set; }
}