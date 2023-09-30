namespace Kira.Pages;

using System.Collections.Immutable;
using Domain;
using Infrastructure.Options;

public partial class FilterForm
{
    public class FormModel
    {
        readonly ICollection<ProjectModel> projects;

        List<ComponentModel> includedComponents = new();
        List<ComponentModel> excludedComponents = new();
        List<TypeModel> includedTypes = new();
        List<TypeModel> excludedTypes = new();
        List<StatusModel> includedStatues = new();
        List<StatusModel> excludedStatues = new();

        public FormModel() : this(Enumerable.Empty<Project>())
        {
        }

        public FormModel(IEnumerable<Project> projects) => this.projects = projects.Select(project => new ProjectModel(project)).ToList();

        public List<ProjectModel>? SelectedProjects { get; set; }
        public List<ComponentModel>? SelectedIncludedComponents { get; set; }
        public List<ComponentModel>? SelectedExcludedComponents { get; set; }
        public List<TypeModel>? SelectedIncludedTypes { get; set; }
        public List<TypeModel>? SelectedExcludedTypes { get; set; }
        public List<StatusModel>? SelectedIncludedStatues { get; set; }
        public List<StatusModel>? SelectedExcludedStatues { get; set; }

        public IEnumerable<ProjectModel> Projects => projects;
        public IEnumerable<ComponentModel> IncludedComponents => FilterByProject(includedComponents);
        public IEnumerable<ComponentModel> ExcludedComponents => FilterByProject(excludedComponents);
        public IEnumerable<TypeModel> IncludedTypes => FilterByProject(includedTypes);
        public IEnumerable<TypeModel> ExcludedTypes => FilterByProject(includedTypes);
        public IEnumerable<StatusModel> IncludedStatues => FilterByProject(includedStatues);
        public IEnumerable<StatusModel> ExcludedStatues => FilterByProject(includedStatues);

        public void Initialize(Defaults defaults)
        {
            var selectedProjects = projects.Where(project => defaults.Projects.Contains(project.Id)).ToList();
            var selectedIncludedComponents = includedComponents.Where(project => defaults.IncludedComponents.Contains(project.Id)).ToList();
            var selectedExcludedComponents = excludedComponents.Where(project => defaults.ExcludedComponents.Contains(project.Id)).ToList();
            var selectedIncludedTypes = includedTypes.Where(project => defaults.IncludedTypes.Contains(project.Id)).ToList();
            var selectedExcludedTypes = excludedTypes.Where(project => defaults.ExcludedTypes.Contains(project.Id)).ToList();
            var selectedIncludedStatues = includedStatues.Where(project => defaults.IncludedStatues.Contains(project.Id)).ToList();
            var selectedExcludedStatues = excludedStatues.Where(project => defaults.ExcludedStatues.Contains(project.Id)).ToList();

            SelectedProjects = selectedProjects;
            SelectedIncludedComponents = selectedIncludedComponents;
            SelectedExcludedComponents = selectedExcludedComponents;
            SelectedIncludedTypes = selectedIncludedTypes;
            SelectedExcludedTypes = selectedExcludedTypes;
            SelectedIncludedStatues = selectedIncludedStatues;
            SelectedExcludedStatues = selectedExcludedStatues;

            List<IFilterModel> acc = new();

            acc.AddRange(includedComponents.Intersect(selectedExcludedComponents));
            acc.AddRange(excludedComponents.Intersect(selectedIncludedComponents));
            acc.AddRange(includedTypes.Intersect(selectedExcludedTypes));
            acc.AddRange(excludedTypes.Intersect(selectedIncludedTypes));
            acc.AddRange(includedStatues.Intersect(selectedExcludedStatues));
            acc.AddRange(excludedStatues.Intersect(selectedIncludedStatues));

            foreach (var model in acc) model.Disable(true);
        }

        public void Add(ProjectModel project, ICollection<Component> components, IEnumerable<ProjectType> projectTypes)
        {
            var includedComponentsModel = components
                .Select(component => new ComponentModel(project, component))
                .ToList();

            var excludedComponentsModel = components
                .Select(component => new ComponentModel(project, component))
                .ToList();

            includedComponents = includedComponents.Concat(includedComponentsModel).OrderBy(component => component.Name).ToList();
            excludedComponents = excludedComponents.Concat(excludedComponentsModel).OrderBy(component => component.Name).ToList();

            var types = projectTypes
                .SelectMany(type => type.Statuses, (type, status) => new
                {
                    IncludedTypeModel = new TypeModel(project, type),
                    ExcludedTypeModel = new TypeModel(project, type),
                    IncludedStatusModel = new StatusModel(project, type, status),
                    ExcludedStatusModel = new StatusModel(project, type, status)
                })
                .ToList();

            var includedTypesModel = types
                .Select(item => item.IncludedTypeModel)
                .DistinctBy(type => type.Id)
                .ToList();

            var excludedTypesModel = types
                .Select(item => item.ExcludedTypeModel)
                .DistinctBy(type => type.Id)
                .ToList();

            includedTypes = includedTypes.Concat(includedTypesModel).OrderBy(type => type.Name).ToList();
            excludedTypes = excludedTypes.Concat(excludedTypesModel).OrderBy(type => type.Name).ToList();

            var includedStatusesModel = types
                .Select(item => item.IncludedStatusModel)
                .DistinctBy(status => status.Id)
                .ToList();

            var excludedStatusesModel = types
                .Select(item => item.ExcludedStatusModel)
                .DistinctBy(status => status.Id)
                .ToList();

            includedStatues = includedStatues.Concat(includedStatusesModel).OrderBy(status => status.Name).ToList();
            excludedStatues = excludedStatues.Concat(excludedStatusesModel).OrderBy(status => status.Name).ToList();
        }

        public void ClearSelected(IEnumerable<ProjectModel> projectsModel)
        {
            var ids = projectsModel.Select(project => project.Id).ToImmutableHashSet();

            SelectedIncludedComponents = SelectedIncludedComponents?.Where(component => ids.Contains(component.Project.Id)).ToList() ?? new();
            SelectedExcludedComponents = SelectedExcludedComponents?.Where(component => ids.Contains(component.Project.Id)).ToList() ?? new();
            SelectedIncludedTypes = SelectedIncludedTypes?.Where(type => ids.Contains(type.Project.Id)).ToList() ?? new();
            SelectedExcludedTypes = SelectedExcludedTypes?.Where(type => ids.Contains(type.Project.Id)).ToList() ?? new();
            SelectedIncludedStatues = SelectedIncludedStatues?.Where(status => ids.Contains(status.Project.Id)).ToList() ?? new();
            SelectedExcludedStatues = SelectedExcludedStatues?.Where(statues => ids.Contains(statues.Project.Id)).ToList() ?? new();
        }

        IEnumerable<T> FilterByProject<T>(IEnumerable<T> models) where T : IFilterModel => models.Where(model => SelectedProjects?.Any(selectedProject => selectedProject.Equals(model.Project)) ?? false).DistinctBy(model => model.Name);
    }
}