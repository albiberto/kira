namespace Kira.Pages;

using Builders;
using Domain;
using Extensions;
using Infrastructure.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen;
using Workloads;

public partial class Rosetta
{
    static readonly string[] Fields = { "id", "key", "assignee", "reporter", "customfield_10421", "duedate", "status", "issuetype", "progress", "parent", "priority", "summary", "labels", "components", "timeoriginalestimate", "timespent", "timeestimate" };

    [Inject] IServiceProvider Provider { get; set; } = null!;
    [Inject] IOptionsSnapshot<CompanyOptions> CompanyOptions { get; set; } = null!;

    IList<LeftModel>? issues;
    int count;

    string LeftKey { get; set; } = string.Empty;
    string RightKey { get; set; } = string.Empty;
    JqlModel Query { get; set; } = new();
    
    async Task LoadData(LoadDataArgs args)
    {
        isLoading = true;

        await Task.Yield();

        Query = Query with { Order = args.ToOrderClause("Assignee") };

        var jira = Provider.GetRequiredKeyedService<JiraService>(LeftKey);
        
        var result = Query.Empty
            ? Array.Empty<Issue>()
            : (await jira.PostSearchAsync(Query.ToJql(), Fields)).ToArray();
        
        var keys = result.Select(issue => $@"summary ~ ""{issue.Key}""");
        var query = string.Join(" OR ", keys);

        var map = await Provider.GetRequiredKeyedService<JiraService>(RightKey).PostSearchAsync($"project = EIMMS AND ({query})", Fields);

        issues = result
        .Select(customerIssue => (CustomerIssue: customerIssue, MyIssues: map.Where(myIssue => myIssue.Fields.Summary.Contains(customerIssue.Key))))
        .Select(issue => new LeftModel(issue.CustomerIssue, issue.MyIssues))
        .ToList();
        
        count = issues.Count;

        isLoading = false;
    }
    
    public Task QueryChanged(JqlModel query)
    {
        Query = query;
        return grid is not null 
            ? grid.Reload() 
            : Task.CompletedTask;
    }
}