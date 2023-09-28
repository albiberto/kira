namespace Kira.Pages;

using System.Collections.Immutable;
using Filter;
using Infrastructure.Clients;
using Infrastructure.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen;

public partial class FilterForm
{
    public enum DropDownListType
    {
        Included = 0,
        Excluded = 1
    }

    readonly HashSet<string> evaluatedProjects = new();
    Designer designer = new();
    bool isLoading = true;
    bool popup;

    [Inject] JiraClient Client { get; set; } = null!;
    [Inject] IOptions<JiraOptions> Options { get; set; } = null!;
    [Inject] NotificationService NotificationService { get; set; } = null!;

    [Parameter] public string Query { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> QueryChanged { get; set; }

    bool DisableSearch => designer.SelectedProjects?.Count is not (>= 0 and <= 3);

    protected override async Task OnInitializedAsync()
    {
        var projects = (await Client.GetAllProjects()).ToList();
        designer = new(projects);

        await LoadProjectAsync(designer.Projects.Where(project => Options.Value.Defaults.Projects.Contains(project.Id)));

        designer.Initialize(Options.Value.Defaults);
    }

    async Task LoadProjectAsync(IEnumerable<ProjectModel>? projects)
    {
        projects ??= Enumerable.Empty<ProjectModel>();

        isLoading = true;

        var toEvaluate = projects.ExceptBy(evaluatedProjects, project => project.Id).ToList();
        foreach (var project in toEvaluate)
        {
            var components = await Client.GetAllProjectsComponents(project.Id);
            var projectTypes = await Client.GetAllProjectStatues(project.Id);

            designer!.Add(project, components.ToList(), projectTypes);
            evaluatedProjects.Add(project.Id);
        }

        isLoading = false;
    }

    public void ChangeComponent(DropDownListType dropDownListType, IEnumerable<ComponentModel>? components)
    {
        var ids = (components ?? Enumerable.Empty<ComponentModel>())
            .Select(component => component.Id)
            .ToImmutableHashSet();

        var toChange = dropDownListType == DropDownListType.Included
            ? designer!.ExcludedComponents
            : designer!.IncludedComponents;

        var toChangeDisable = toChange
            .Where(c => ids.Contains(c.Id))
            .Where(c => !c.Disabled);

        foreach (var component in toChangeDisable) component.Disable(true);

        var toChangeEnable = toChange
            .Where(c => !ids.Contains(c.Id))
            .Where(c => c.Disabled);

        foreach (var component in toChangeEnable) component.Disable(false);
    }

    public void ChangeType(DropDownListType dropDownListType, IEnumerable<TypeModel>? types)
    {
        var ids = (types ?? Enumerable.Empty<TypeModel>())
            .Select(type => type.Id)
            .ToImmutableHashSet();

        var toChange = dropDownListType == DropDownListType.Included
            ? designer!.ExcludedTypes
            : designer!.IncludedTypes;

        var toChangeDisable = toChange
            .Where(t => ids.Contains(t.Id))
            .Where(t => !t.Disabled);

        foreach (var typeModel in toChangeDisable) typeModel.Disable(true);

        var toChangeEnable = toChange
            .Where(t => !ids.Contains(t.Id))
            .Where(t => t.Disabled);

        foreach (var typeModel in toChangeEnable) typeModel.Disable(false);
    }

    public void ChangeStatus(DropDownListType dropDownListType, IEnumerable<StatusModel>? statues)
    {
        var ids = (statues ?? Enumerable.Empty<StatusModel>())
            .Select(status => status.Id)
            .ToImmutableHashSet();

        var toChange = dropDownListType == DropDownListType.Included
            ? designer!.ExcludedStatues
            : designer!.IncludedStatues;

        var toChangeDisable = toChange
            .Where(t => ids.Contains(t.Id))
            .Where(t => !t.Disabled);

        foreach (var typeModel in toChangeDisable) typeModel.Disable(true);

        var toChangeEnable = toChange
            .Where(t => !ids.Contains(t.Id))
            .Where(t => t.Disabled);

        foreach (var typeModel in toChangeEnable) typeModel.Disable(false);
    }

    async Task OnSubmit()
    {
        var jql = designer?.ToJql() ?? string.Empty;

        Query = jql;
        await QueryChanged.InvokeAsync(jql);

        NotificationService.Notify(NotificationSeverity.Success, "Success", "Form submitted successfully!");
    }

    void OnInvalidSubmit() => NotificationService.Notify(NotificationSeverity.Error, "Error", "Form has errors!");
}