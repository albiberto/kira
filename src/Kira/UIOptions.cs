namespace Kira;

using System.ComponentModel.DataAnnotations;

public class UiOptions
{
    [Required] public FilterOptions Filter { get; set; } = null!;
}

public class FilterOptions
{
    [Required, MinLength(3)] public string Text { get; set; } = null!;
    [Required, MinLength(3)] public string Icon { get; set; } = null!;
}