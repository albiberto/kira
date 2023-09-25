namespace Kira.Domain;

using System.Text.Json.Serialization;

public record Vote
{
    [JsonPropertyName("self")] public string Self { get; set; } = string.Empty;
    [JsonPropertyName("votes")] public int Votes { get; set; }
    [JsonPropertyName("hasVoted")] public bool HasVoted { get; set; }
}