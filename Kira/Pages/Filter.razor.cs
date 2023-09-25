namespace Kira.Pages;

using System.Collections.Immutable;
using Builders;
using Comparers.Filter;
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
    [Inject] StatusModelComparer StatusComparer { get; set; } = null!;
    [Inject] TypeModelComparer TypeComparer { get; set; } = null!;

    [Parameter] public string Query { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> QueryChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var options = Options.Value.Projects.ToList();
        var defaultProjects = options.Select(o => o.Project).ToImmutableHashSet();
        var defaultIncludedComponents = options.SelectMany(o => o.IncludedComponents).ToImmutableHashSet();
        var defaultExcludedComponents = options.SelectMany(o => o.ExcludedComponents).ToImmutableHashSet();
        var defaultIncludedTypes = options.SelectMany(o => o.IncludedTypes.Select(type => type.Type)).ToImmutableHashSet();
        var defaultExcludedTypes = options.SelectMany(o => o.ExcludedTypes.Select(type => type.Type)).ToImmutableHashSet();
        var defaultIncludedStatues = options.SelectMany(o => o.IncludedTypes.SelectMany(type => type.IncludedStatus.Concat(type.ExcludedStatus))).ToImmutableHashSet();
        var defaultExcludedStatues = options.SelectMany(o => o.ExcludedTypes.SelectMany(type => type.IncludedStatus.Concat(type.ExcludedStatus))).ToImmutableHashSet();

        var projects = await Client.GetAllProjects();

        designer = new(projects, defaultProjects, defaultIncludedComponents, defaultExcludedComponents, defaultIncludedTypes, defaultExcludedTypes, defaultIncludedStatues, defaultExcludedStatues);

        await BuildModelAsync();
    }

    async Task BuildModelAsync()
    {
        isLoading = true;

        foreach (var project in designer.ToEvaluate())
        {
            var components = await Client.GetAllProjectsComponents(project.Id);
            var projectTypes = await Client.GetAllProjectStatues(project.Id);

            designer.Add(project, components.ToList(), projectTypes.ToList());
        }

        await FireDataBindingAsync();

        isLoading = false;
    }

    async Task ProjectChangeAsync(object _) => await BuildModelAsync();

    async Task ComponentChangeAsync(Designer.DropDownListType ddlType, object components)
    {
        designer.ChangeComponent(ddlType, (IEnumerable<string>)components);
        await FireDataBindingAsync();
    }

    async Task TypeChangeAsync(Designer.DropDownListType ddlType, object type)
    {
        designer.ChangeType(ddlType, (IEnumerable<string>)type);
        await FireDataBindingAsync();
    }

    async Task StatusChangeAsync(Designer.DropDownListType ddlType, object status)
    {
        designer.ChangeStatus(ddlType, (IEnumerable<string>)status);
        await FireDataBindingAsync();
    }

    async Task FireDataBindingAsync()
    {
        var jql = Builder.BuildJqlQuery(designer.SelectedProjectIds,
            designer.SelectedIncludedComponents,
            designer.SelectedExcludedComponents,
            designer.SelectedIncludedTypes,
            designer.SelectedExcludedTypes,
            designer.SelectedIncludedStatues,
            designer.SelectedExcludedStatues);

        Query = jql;
        await QueryChanged.InvokeAsync(jql);
    }
}