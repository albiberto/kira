namespace Kira.Pages;

using Domain;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Workload
{
    [Inject] TooltipService TooltipService { get; set; } = null!;

    RadzenDataGrid<Issue> grid = null!;

    bool groupsExpanded = true;
    bool isLoading = true;
    
    void OnRender(DataGridRenderEventArgs<Issue> args)
    {
        if (!args.FirstRender) return;

        args.Grid.Groups.Add(new() { Property = "Fields.Assignee.DisplayName", Title = "Assignee" });
        StateHasChanged();
    }
    
    void OnGroupRowRender(GroupRowRenderEventArgs args) => args.Expanded = groupsExpanded;
    
    void ShowTooltip(ElementReference elementReference, string text, TooltipOptions? options = default) => TooltipService.Open(elementReference, text, options);
}