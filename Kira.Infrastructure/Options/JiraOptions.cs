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
    [Required] public HashSet<ProjectOptions> Projects { get; set; } = new();
}

public class ProjectOptions
{
    [Required, MinLength(3)] public string Project { get; set; } = null!;

    [Required] public HashSet<string> IncludedComponents { get; set; } = new();
    [Required] public HashSet<string> ExcludedComponents { get; set; } = new();
}