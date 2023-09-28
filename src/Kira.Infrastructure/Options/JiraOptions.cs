namespace Kira.Infrastructure.Options;

using System.ComponentModel.DataAnnotations;

public class AuthOptions
{
    [Required, MinLength(10)] public string Username { get; set; } = null!;
    [Required, MinLength(6)] public string Password { get; set; } = null!;
}

public class JiraOptions
{
    [Required, MinLength(6)] public string BaseAddress { get; set; } = null!;

    [Required, MinLength(6)] public string UIAddress { get; set; } = null!;
    [Required] public Defaults Defaults { get; set; } = new();
}

public class Defaults
{
    [Required] public HashSet<string> Projects { get; set; } = new();
    [Required] public HashSet<string> IncludedComponents { get; set; } = new();
    [Required] public HashSet<string> ExcludedComponents { get; set; } = new();
    [Required] public HashSet<string> IncludedTypes { get; set; } = new();
    [Required] public HashSet<string> ExcludedTypes { get; set; } = new();
    [Required] public HashSet<string> IncludedStatues { get; set; } = new();
    [Required] public HashSet<string> ExcludedStatues { get; set; } = new();
}