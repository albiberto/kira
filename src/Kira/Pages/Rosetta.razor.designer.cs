namespace Kira.Pages;

using Infrastructure.Options;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Rosetta
{
    bool isLoading = true;
    bool? allGroupsExpanded;

    RadzenDataGrid<LeftModel> grid;
    
    [Inject] TooltipService TooltipService { get; set; } = null!;
    
    void RowRender(RowRenderEventArgs<LeftModel> args) => args.Expandable = true;

    protected override async Task OnAfterRenderAsync(bool firstRender) => await base.OnAfterRenderAsync(firstRender);
}