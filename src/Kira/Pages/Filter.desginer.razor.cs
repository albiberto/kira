namespace Kira.Pages;

using System.Collections.Immutable;
using Comparers.Filter;
using Domain;

public partial class Filter
{
    public class Designer(
        IEnumerable<Project>? projects = default,
        ImmutableHashSet<string>? defaultProjects = default,
        ImmutableHashSet<string>? defaultIncludedComponents = default,
        ImmutableHashSet<string>? defaultExcludedComponents = default,
        ImmutableHashSet<string>? defaultIncludedTypes = default,
        ImmutableHashSet<string>? defaultExcludedTypes = default,
        ImmutableHashSet<string>? defaultIncludedStatues = default,
        ImmutableHashSet<string>? defaultExcludedStatues = default)
    {
        public enum DropDownListType
        {
            Included = 0,
            Excluded = 1
        }

        readonly List<ComponentModel> excludedComponents = new();

        readonly List<StatusModel> excludedStatues = new();

        readonly List<TypeModel> excludedTypes = new();
        readonly List<ComponentModel> includedComponents = new();
        readonly List<StatusModel> includedStatues = new();
        readonly List<TypeModel> includedTypes = new();

        HashSet<string> EvaluatedProjects { get; set; } = new();

        public List<string>? SelectedProjectIds { get; set; } = defaultProjects?.ToList() ?? new();

        public List<string>? SelectedIncludedComponentIds { get; set; } = defaultIncludedComponents?.ToList() ?? new();
        public List<string>? SelectedExcludedComponentIds { get; set; } = defaultExcludedComponents?.ToList() ?? new();

        public List<string>? SelectedIncludedTypeIds { get; set; } = defaultIncludedTypes?.ToList() ?? new();
        public List<string>? SelectedExcludedTypeIds { get; set; } = defaultExcludedTypes?.ToList() ?? new();

        public List<string>? SelectedIncludedStatusIds { get; set; } = defaultIncludedStatues?.ToList() ?? new();
        public List<string>? SelectedExcludedStatusIds { get; set; } = defaultExcludedStatues?.ToList() ?? new();

        public IEnumerable<ProjectModel> Projects { get; } = projects?
            .Select(project => new ProjectModel(project))
            .OrderBy(project => project.Key)
            .ToHashSet() ?? new();

        public IEnumerable<ComponentModel> IncludedComponents => includedComponents
            .Where(component => SelectedProjectIds?.Contains(component.ProjectId) ?? false)
            .OrderBy(type => type.Name);

        public IEnumerable<ComponentModel> ExcludedComponents => excludedComponents
            .Where(component => SelectedProjectIds?.Contains(component.ProjectId) ?? false)
            .OrderBy(type => type.Name);

        public IEnumerable<TypeModel> IncludedTypes => includedTypes
            .Where(type => SelectedProjectIds?.Contains(type.ProjectId) ?? false)
            .OrderBy(type => type.Name)
            .Distinct(new TypeModelComparer());

        public IEnumerable<TypeModel> ExcludedTypes => excludedTypes
            .Where(type => SelectedProjectIds?.Contains(type.ProjectId) ?? false)
            .OrderBy(type => type.Name)
            .Distinct(new TypeModelComparer());

        public IEnumerable<StatusModel> IncludedStatues => includedStatues
            .Where(status => SelectedProjectIds?.Contains(status.ProjectId) ?? false)
            .OrderBy(type => type.Name)
            .Distinct(new StatusModelComparer());

        public IEnumerable<StatusModel> ExcludedStatues => excludedStatues
            .Where(status => SelectedProjectIds?.Contains(status.ProjectId) ?? false)
            .OrderBy(type => type.Name)
            .Distinct(new StatusModelComparer());

        public IEnumerable<(string ProjectId, string componentId)> SelectedIncludedComponents => IncludedComponents
            .Where(component => SelectedIncludedComponentIds?.Contains(component.Id) ?? false)
            .Select(component => (component.ProjectId, component.Id));

        public IEnumerable<(string ProjectId, string Id)> SelectedExcludedComponents => ExcludedComponents
            .Where(component => SelectedExcludedComponentIds?.Contains(component.Id) ?? false)
            .Select(component => (component.ProjectId, component.Id));

        public IEnumerable<(string ProjectId, string componentId)> SelectedIncludedTypes => IncludedTypes
            .Where(type => SelectedIncludedTypeIds?.Contains(type.Id) ?? false)
            .Select(type => (type.ProjectId, type.Id));

        public IEnumerable<(string ProjectId, string Id)> SelectedExcludedTypes => ExcludedTypes
            .Where(typeModel => SelectedExcludedTypeIds?.Contains(typeModel.Id) ?? false)
            .Select(type => (type.ProjectId, type.Id));

        public IEnumerable<(string ProjectId, string componentId)> SelectedIncludedStatues => IncludedStatues
            .Where(component => SelectedIncludedStatusIds?.Contains(component.Id) ?? false)
            .Select(component => (component.ProjectId, component.Id));

        public IEnumerable<(string ProjectId, string Id)> SelectedExcludedStatues => ExcludedStatues
            .Where(component => SelectedExcludedStatusIds?.Contains(component.Id) ?? false)
            .Select(component => (component.ProjectId, component.Id));

        public IEnumerable<ProjectModel> ToEvaluate() => Projects
            .Where(project => (SelectedProjectIds ?? Enumerable.Empty<string>()).Contains(project.Id))
            .Where(project => !EvaluatedProjects.Contains(project.Id));

        public void Add(ProjectModel project, List<Component> components, List<ProjectType> projectTypes)
        {
            includedComponents.AddRange(components.Select(component => new ComponentModel(project, component, defaultExcludedComponents?.Contains(component.Id) ?? false)));
            excludedComponents.AddRange(components.Select(component => new ComponentModel(project, component, defaultIncludedComponents?.Contains(component.Id) ?? false)));

            includedTypes.AddRange(projectTypes.Select(type => new TypeModel(project, type, defaultExcludedTypes?.Contains(type.Id) ?? false)));
            excludedTypes.AddRange(projectTypes.Select(type => new TypeModel(project, type, defaultIncludedTypes?.Contains(type.Id) ?? false)));

            includedStatues.AddRange(projectTypes.SelectMany(type => type.Statuses.Select(status => new StatusModel(project, type, status, defaultExcludedStatues?.Contains(status.Id) ?? false))));
            excludedStatues.AddRange(projectTypes.SelectMany(type => type.Statuses.Select(status => new StatusModel(project, type, status, defaultIncludedStatues?.Contains(status.Id) ?? false))));

            EvaluatedProjects.Add(project.Id);
        }

        public void ChangeComponent(DropDownListType dropDownListType, IEnumerable<string>? components)
        {
            components ??= Enumerable.Empty<string>();

            var toChange = dropDownListType == DropDownListType.Included
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

        public void ChangeType(DropDownListType dropDownListType, IEnumerable<string>? types)
        {
            types ??= Enumerable.Empty<string>();

            var toChange = dropDownListType == DropDownListType.Included
                ? excludedTypes
                : includedTypes;

            var toChangeDisable = toChange
                .Where(t => types.Contains(t.Id))
                .Where(t => !t.Disabled);

            foreach (var typeModel in toChangeDisable) typeModel.Disable(true);

            var toChangeEnable = toChange
                .Where(t => !types.Contains(t.Id))
                .Where(t => t.Disabled);

            foreach (var typeModel in toChangeEnable) typeModel.Disable(false);
        }

        public void ChangeStatus(DropDownListType dropDownListType, IEnumerable<string>? statues)
        {
            statues ??= Enumerable.Empty<string>();

            var toChange = dropDownListType == DropDownListType.Included
                ? excludedStatues
                : includedStatues;

            var toChangeDisable = toChange
                .Where(t => statues.Contains(t.Id))
                .Where(t => !t.Disabled);

            foreach (var typeModel in toChangeDisable) typeModel.Disable(true);

            var toChangeEnable = toChange
                .Where(t => !statues.Contains(t.Id))
                .Where(t => t.Disabled);

            foreach (var typeModel in toChangeEnable) typeModel.Disable(false);
        }
    }
}