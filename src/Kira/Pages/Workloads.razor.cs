namespace Kira.Pages;

using Builders;
using Domain;
using Extensions;
using Infrastructure.Clients;
using Microsoft.AspNetCore.Components;
using Radzen;

public partial class Workloads
{
    static readonly string[] Fields = { "id", "key", "assignee", "reporter", "customfield_10421", "duedate", "status", "issuetype", "progress", "parent", "priority", "summary", "labels", "components", "timeoriginalestimate", "timespent", "timeestimate" };

    [Inject] JiraClient Client { get; set; } = null!;

    IList<Model>? issues;
    int count;

    JqlModel Query { get; set; } = new();

    async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;

        await Task.Yield();

        Query = Query with { Order = args.ToOrderClause("Assignee") };

        var result = Query.Empty
            ? Enumerable.Empty<Issue>()
            : await Client.PostSearchAsync(Query.ToJql(), Fields);

        issues = result.Select(model => new Model(model)).ToList();
        count = issues.Count;

        isLoading = false;
    }

    public async Task QueryChanged(JqlModel query)
    {
        Query = query;
        if (grid is not null) await grid.Reload();
    }
}