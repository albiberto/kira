namespace Kira.Pages;

using Domain;
using Infrastructure.Clients;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Workload
{
    [Inject] JiraClient Client { get; set; } = null!;
    
    string Query { get; set; } = string.Empty;
    static readonly string[] Fields = { "id", "key", "assignee", "reporter", "status", "issuetype", "progress", "parent", "priority", "summary", "labels", "components", "timeoriginalestimate", "timespent", "timeestimate" };

    bool isLoading = true;
    int count;
    IList<Issue>? issues;
    RadzenDataGrid<Issue> grid = null!;


    async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;

        await Task.Yield();
        await Task.Delay(8000);

        var result = await Client.PostSearchAsync(Query, Fields);

        // var result = new List<Issue>();
        
        issues = result.OrderBy(issue => issue.Fields.Assignee?.EmailAddress ?? "Unassigned").ToList();
        count = issues.Count;
        
        isLoading = false;
    }

    void OnRender(DataGridRenderEventArgs<Issue> args)
    {
        if (!args.FirstRender) return;
        
        args.Grid.Groups.Add(new(){ Property = "Fields.Assignee.EmailAddress", Title = "Email" });
        StateHasChanged();
    }
}