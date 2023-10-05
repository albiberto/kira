namespace Kira.Pages;

using System.Collections.Immutable;
using Builders;
using Infrastructure.Clients;
using Infrastructure.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen;

public partial class Filter
{
    readonly HashSet<string> evaluatedProjects = new();

    [Inject] JiraClient Client { get; set; } = null!;
    [Inject] IOptions<JiraOptions> Options { get; set; } = null!;
    [Inject] NotificationService NotificationService { get; set; } = null!;

    [Parameter] public JqlModel Query { get; set; } = new();
    [Parameter] public EventCallback<JqlModel> QueryChanged { get; set; }

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

    void IncludedComponentsChange(object? args) => Change(args, formModel.ExcludedComponents);
    void ExcludedComponentsChange(object? args) => Change(args, formModel.IncludedComponents);

    void IncludedTypesChange(object? args) => Change(args, formModel.ExcludedTypes);
    void ExcludedTypesChange(object? args) => Change(args, formModel.IncludedTypes);

    void IncludedStatuesChange(object? args) => Change(args, formModel.ExcludedStatues);
    void ExcludedStatuesChange(object? args) => Change(args, formModel.IncludedStatues);

    static void Change<T>(
        object? changedSelection, IEnumerable<T> listToUpdate
        ) where T : IFilterModel
    {
        //These are all the selected IDs in the list that just changed, so they should be disabled in the other list
        var idsThatShouldBeDisabled = (changedSelection as IEnumerable<T> ?? Enumerable.Empty<T>())
            .Select(x => x.Id)
            .ToImmutableHashSet();

        foreach (var typeModel in listToUpdate)
        {
            if (idsThatShouldBeDisabled.Contains(typeModel.Id))
                typeModel.Disable(true);
            else
                //All IDs that are not selected in one list should be enabled in the other
                typeModel.Disable(false);
        }
    }

    async Task OnSubmit()
    {
        Query = formModel;
        await QueryChanged.InvokeAsync(formModel);

        NotificationService.Notify(NotificationSeverity.Success, "Success", "Form submitted successfully!");
    }

    void OnInvalidSubmit() => NotificationService.Notify(NotificationSeverity.Error, "Error", "Form has errors!");
}