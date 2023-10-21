namespace Kira.Pages.Workloads;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen;
using Radzen.Blazor;

public partial class Company1
{
    RadzenDataGrid<Model> grid;

    bool isLoading = true;
    bool? allGroupsExpanded;

    UiOptions? ui;
    
    [Inject] IOptions<UiOptions> UIOptions { get; set; } = null!;
    [Inject] TooltipService TooltipService { get; set; } = null!;

    void OnRender(DataGridRenderEventArgs<Model> args)
    {
        if (!args.FirstRender) return;

        args.Grid.Groups.Add(new() { Property = "Assignee", SortOrder = SortOrder.Descending, Title = "Assignee" });
        StateHasChanged();
    }
    
    void OnGroupRowRender(GroupRowRenderEventArgs args) => args.Expanded = allGroupsExpanded;
    
    void ShowTooltip(ElementReference elementReference, string text, TooltipOptions? options = default) => TooltipService.Open(elementReference, text, options);
}