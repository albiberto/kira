namespace Kira.Pages;

using System.Collections.Immutable;
using Builders;
using Infrastructure.Clients;
using Infrastructure.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

public partial class Filter
{
    Designer designer = new();
    bool isLoading = true;

    [Inject] JiraClient Client { get; set; } = null!;
    [Inject] IOptions<JiraOptions> Options { get; set; } = null!;
    [Inject] FilterBuilder Builder { get; set; } = null!;

    [Parameter] public string Query { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> QueryChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var options = Options.Value.Projects.ToList();
        var defaultProjects = options.Select(o => o.Project).ToImmutableHashSet();
        var defaultIncludedComponents = options.SelectMany(o => o.IncludedComponents).ToImmutableHashSet();
        var defaultExcludedComponents = options.SelectMany(o => o.ExcludedComponents).ToImmutableHashSet();

        var projects = await Client.GetAllProjects();

        designer = new(projects, defaultProjects, defaultIncludedComponents, defaultExcludedComponents);

        await BuildComponentsModelAsync();
    }

    async Task BuildComponentsModelAsync()
    {
        isLoading = true;

        foreach (var project in designer.ToEvaluate())
        {
            var components = await Client.GetAllProjectsComponents(project.Id);
            designer.Add(project, components.ToList());
        }

        await FireDataBindingAsync();

        isLoading = false;
    }

    async Task ProjectChangeAsync(object _) => await BuildComponentsModelAsync();

    async Task ComponentChangeAsync(Designer.Type type, object components)
    {
        designer.Change(type, (IEnumerable<string>)components);
        await FireDataBindingAsync();
    }

    async Task FireDataBindingAsync()
    {
        var jql = Builder.BuildJqlQuery(designer.SelectedProjectIds, designer.SelectedIncludedComponents, designer.SelectedExcludedComponents);

        Query = jql;
        await QueryChanged.InvokeAsync(jql);
    }
}