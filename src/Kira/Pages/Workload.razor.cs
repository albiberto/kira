namespace Kira.Pages;

using Builders;
using Domain;
using Extensions;
using Infrastructure.Clients;
using Microsoft.AspNetCore.Components;
using Radzen;

public partial class Workload
{
    [Inject] JiraClient Client { get; set; } = null!;
    static readonly string[] Fields = { "id", "key", "assignee", "reporter", "customfield_10421", "duedate", "status", "issuetype", "progress", "parent", "priority", "summary", "labels", "components", "timeoriginalestimate", "timespent", "timeestimate" };
    
    IList<Issue>? issues;
    int count;
    
    JqlModel Query { get; set; } = new();

    async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;

        await Task.Yield();

        Query = Query with { Order = args.ToOrderClause() };

        var result = Query.Empty
            ? Enumerable.Empty<Issue>()
            : await Client.PostSearchAsync(Query.ToJql(), Fields);

        issues = result.OrderBy(issue => issue.Fields.Assignee?.EmailAddress ?? "Unassigned").ToList();
        count = issues.Count;

        isLoading = false;
    }
    
    public async Task QueryChanged(JqlModel query)
    {
        Query = query;
        await grid.Reload();
    }
}