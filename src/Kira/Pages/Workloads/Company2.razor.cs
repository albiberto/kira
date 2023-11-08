namespace Kira.Pages.Workloads;

using Builders;
using Domain;
using Extensions;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen;

public partial class Company2
{
    static readonly string[] Fields = { "id", "key", "assignee", "reporter", "customfield_10421", "duedate", "status", "issuetype", "progress", "parent", "priority", "summary", "labels", "components", "timeoriginalestimate", "timespent", "timeestimate" };

    [Inject] IServiceProvider Provider { get; set; } = null!;
    [Inject] IOptionsSnapshot<CompanyOptions> OptionsSnapshot { get; set; } = null!;
    
    const string Key = nameof(Company2);
    JiraService jira = null!;
    JiraOptions options = null!;
    
    IList<Model>? issues;
    int count;

    JqlModel Query { get; set; } = new();

    protected override void OnInitialized()
    {
        jira = Provider.GetRequiredKeyedService<JiraService>(Key);
        options = OptionsSnapshot.Get(Key).Jira;
    }
    
    async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;

        await Task.Yield();

        Query = Query with { Order = args.ToOrderClause("Assignee") };

        var result = Query.Empty
            ? Enumerable.Empty<Issue>()
            : await jira.PostSearchAsync(Query.ToJql(), Fields);

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