namespace Kira.Pages;

using System.Collections.Immutable;
using Domain;

public partial class Filter
{
    public class Designer(IEnumerable<Project>? projects = default, ImmutableHashSet<string>? defaultProjects = default, ImmutableHashSet<string>? defaultIncludedComponents = default, ImmutableHashSet<string>? defaultExcludedComponents = default)
    {
        public enum Type
        {
            Included = 0,
            Excluded = 1
        }

        readonly List<ComponentModel> excludedComponents = new();
        readonly List<ComponentModel> includedComponents = new();
        HashSet<string> EvaluatedProjects { get; set; } = new();

        public List<string>? SelectedProjectIds { get; set; } = defaultProjects?.ToList() ?? new();
        public List<string>? SelectedIncludedComponentIds { get; set; } = defaultIncludedComponents?.ToList() ?? new();
        public List<string>? SelectedExcludedComponentIds { get; set; } = defaultExcludedComponents?.ToList() ?? new();

        public IEnumerable<ProjectModel> Projects { get; } = projects?
            .Select(project => new ProjectModel(project))
            .OrderBy(project => project.Key)
            .ToHashSet() ?? new();

        public IEnumerable<ComponentModel> IncludedComponents => includedComponents.Where(component => SelectedProjectIds?.Contains(component.ProjectId) ?? false);
        public IEnumerable<ComponentModel> ExcludedComponents => excludedComponents.Where(component => SelectedProjectIds?.Contains(component.ProjectId) ?? false);

        public IEnumerable<(string ProjectId, string componentId)> SelectedIncludedComponents => IncludedComponents
            .Where(component => SelectedIncludedComponentIds?.Contains(component.Id) ?? false)
            .Select(component => (component.ProjectId, component.Id));

        public IEnumerable<(string ProjectId, string Id)> SelectedExcludedComponents => ExcludedComponents
            .Where(component => SelectedExcludedComponentIds?.Contains(component.Id) ?? false)
            .Select(component => (component.ProjectId, component.Id));

        public IEnumerable<ProjectModel> ToEvaluate() => Projects
            .Where(project => (SelectedProjectIds ?? Enumerable.Empty<string>()).Contains(project.Id))
            .Where(project => !EvaluatedProjects.Contains(project.Id));

        public void Add(ProjectModel project, IEnumerable<Component> components)
        {
            var resolved = components.ToHashSet();

            includedComponents.AddRange(resolved.Select(component => new ComponentModel(project, component, (defaultExcludedComponents ?? ImmutableHashSet<string>.Empty).Contains(component.Id))));
            excludedComponents.AddRange(resolved.Select(component => new ComponentModel(project, component, (defaultIncludedComponents ?? ImmutableHashSet<string>.Empty).Contains(component.Id))));

            EvaluatedProjects.Add(project.Id);
        }

        public void Change(Type type, IEnumerable<string>? components)
        {
            components ??= Enumerable.Empty<string>();

            var toChange = type == Type.Included
                ? excludedComponents
                : includedComponents;

            var toChangeDisable = toChange
                .Where(c => components.Contains(c.Id))
                .Where(c => !c.Disabled);

            foreach (var component in toChangeDisable) component.Disable(true);

            var toChangeEnable = toChange
                .Where(c => !components.Contains(c.Id))
                .Where(c => c.Disabled);

            foreach (var component in toChangeEnable) component.Disable(false);
        }
    }
}