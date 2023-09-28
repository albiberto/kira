namespace Kira.Pages;

using Domain;
using Infrastructure.Clients;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Workload
{
    static readonly string[] Fields = { "id", "key", "assignee", "reporter", "status", "issuetype", "progress", "parent", "priority", "summary", "labels", "components", "timeoriginalestimate", "timespent", "timeestimate" };
    int count;
    RadzenDataGrid<Issue> grid = null!;

    bool isLoading = true;
    IList<Issue>? issues;
    [Inject] JiraClient Client { get; set; } = null!;

    string Query { get; set; } = string.Empty;


    public async Task CallBack(string query)
    {
        Query = query;
        await grid.Reload();
    }

    async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;

        await Task.Yield();

        var result = await Client.PostSearchAsync(Query, Fields);

        // var result = new List<Issue>();

        issues = result.OrderBy(issue => issue.Fields.Assignee?.EmailAddress ?? "Unassigned").ToList();
        count = issues.Count;

        isLoading = false;
    }

    void OnRender(DataGridRenderEventArgs<Issue> args)
    {
        if (!args.FirstRender) return;

        args.Grid.Groups.Add(new() { Property = "Fields.Assignee.EmailAddress", Title = "Email" });
        StateHasChanged();
    }
}