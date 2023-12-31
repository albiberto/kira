﻿namespace Kira.Pages.Workloads;

using System.Collections;
using Extensions;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Company2
{
    bool? allGroupsExpanded;

    RadzenDataGrid<Model>? grid;

    bool isLoading = true;
    [Inject] TooltipService TooltipService { get; set; } = null!;

    void OnRender(DataGridRenderEventArgs<Model> args)
    {
        if (!args.FirstRender) return;

        args.Grid.Groups.Add(new() { Property = "Assignee", SortOrder = SortOrder.Descending, Title = "Assignee" });
        StateHasChanged();
    }

    void ShowTooltip(ElementReference elementReference, string text, TooltipOptions? options = default) => TooltipService.Open(elementReference, text, options);

    void OnGroupRowRender(GroupRowRenderEventArgs args) => args.Expanded = allGroupsExpanded;

    static string ToWorkingHoursDaysMinutes(IEnumerable items) => items.Cast<Model>().Sum(o => o.RemainingEstimate).ToWorkingDaysHoursMinutes();
}