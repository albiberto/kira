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
    readonly HashSet<string> evaluatedProjects = new();

    [Inject] JiraClient Client { get; set; } = null!;
    [Inject] IOptions<JiraOptions> Options { get; set; } = null!;
    [Inject] NotificationService NotificationService { get; set; } = null!;

    [Parameter] public string Query { get; set; } = string.Empty;
    [Parameter] public EventCallback<string> QueryChanged { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var projects = (await Client.GetAllProjects()).ToList();
        formModel = new(projects);

        await LoadProjectAsync(formModel.Projects.Where(project => Options.Value.Defaults.Projects.Contains(project.Id)));

        formModel.Initialize(Options.Value.Defaults);
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

            formModel.Add(project, components.ToList(), projectTypes);
            evaluatedProjects.Add(project.Id);
        }

        isLoading = false;
    }

    async Task ProjectChange(object? args)
    {
        var projects = (args as IEnumerable<ProjectModel> ?? Enumerable.Empty<ProjectModel>()).ToList();
        
        await LoadProjectAsync(projects);
        formModel.ClearSelected(projects);
    }
    
    void IncludedComponentsChange(object? args) => Change(formModel.ExcludedComponents, formModel.IncludedComponents, args);
    void ExcludedComponentsChange(object? args) => Change(formModel.IncludedComponents, formModel.ExcludedComponents, args);
    
    void IncludedTypesChange(object? args) => Change(formModel.ExcludedTypes, formModel.IncludedTypes, args);
    void ExcludedTypesChange(object? args) => Change(formModel.IncludedTypes, formModel.ExcludedTypes, args);
    
    void IncludedStatuesChange(object? statues) => Change(formModel.ExcludedStatues, formModel.IncludedStatues, statues);
    void ExcludedStatuesChange(object? statues) => Change(formModel.IncludedStatues, formModel.ExcludedStatues, statues);

    static void Change<T>(IEnumerable<T> toDisable, IEnumerable<T> toEnable, object? args) where T: IFilterModel
    {
        var ids = (args as IEnumerable<T> ?? Enumerable.Empty<T>())
            .Select(status => status.Id)
            .ToImmutableHashSet();

        var toChangeDisable = toDisable
            .Where(t => ids.Contains(t.Id))
            .Where(t => !t.Disabled);

        foreach (var typeModel in toChangeDisable) typeModel.Disable(true);

        var toChangeEnable = toEnable
            .Where(t => !ids.Contains(t.Id))
            .Where(t => t.Disabled);

        foreach (var typeModel in toChangeEnable) typeModel.Disable(false);
    }

    async Task OnSubmit()
    {
        var jql = formModel?.ToJql() ?? string.Empty;

        Query = jql;
        await QueryChanged.InvokeAsync(jql);

        NotificationService.Notify(NotificationSeverity.Success, "Success", "Form submitted successfully!");
    }

    void OnInvalidSubmit() => NotificationService.Notify(NotificationSeverity.Error, "Error", "Form has errors!");
}