namespace Kira.Domain;

using System.Text.Json.Serialization;

public record AvatarUrls
{
    [JsonPropertyName("48x48")] public string _48x48 { get; set; } = string.Empty;
    [JsonPropertyName("24x24")] public string _24x24 { get; set; } = string.Empty;
    [JsonPropertyName("16x16")] public string _16x16 { get; set; } = string.Empty;
    [JsonPropertyName("32x32")] public string _32x32 { get; set; } = string.Empty;
}