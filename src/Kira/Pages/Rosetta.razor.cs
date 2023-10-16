namespace Kira.Pages;

using Builders;
using Infrastructure.Clients;
using Infrastructure.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen.Blazor;

public partial class Rosetta
{
    JqlModel Query { get; set; } = new();
    
    static readonly string[] Fields = { "id", "key", "assignee", "reporter", "customfield_10421", "duedate", "status", "issuetype", "progress", "parent", "priority", "summary", "labels", "components", "timeoriginalestimate", "timespent", "timeestimate" };

    [Inject] JiraClient.My  MyClient { get; set; } = null!;
    [Inject] JiraClient.Customer  CustomerClient { get; set; } = null!;
    [Inject] IOptions<BoardOptions.My> MyOptions { get; set; } = null!;
    [Inject] IOptions<BoardOptions.Customer> CustomerOptions { get; set; } = null!;
    
    public void QueryChanged(JqlModel query)
    {
        Query = query;
    }
}