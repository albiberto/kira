namespace Kira.Pages.Filters;

using Microsoft.AspNetCore.Components;

public partial class Collapse
{
    [Parameter] public bool? Value { get; set; }
    [Parameter] public EventCallback<bool?> ValueChanged { get; set; }

    Task ToggleGroups(bool? value)
    {
        Value = value;
        return ValueChanged.InvokeAsync(value);
    }
}