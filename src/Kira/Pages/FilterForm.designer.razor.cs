namespace Kira.Pages;

using Domain;
using Infrastructure.Options;

public partial class FilterForm
{
    public class Designer(IEnumerable<Project> projects)
    {
        readonly List<ProjectModel> projects = projects.Select(project => new ProjectModel(project)).ToList();
        List<ComponentModel> excludedComponents = new();
        List<StatusModel> excludedStatues = new();
        List<TypeModel> excludedTypes = new();

        List<ComponentModel> includedComponents = new();
        List<StatusModel> includedStatues = new();
        List<TypeModel> includedTypes = new();

        public Designer() : this(Enumerable.Empty<Project>())
        {
        }

        public List<ProjectModel> Projects => projects.ToList();

        public IEnumerable<ComponentModel> IncludedComponents => includedComponents.Where(component => SelectedProjects?.Any(selectedProject => selectedProject.Equals(component.Project)) ?? false);
        public IEnumerable<ComponentModel> ExcludedComponents => excludedComponents.Where(component => SelectedProjects?.Any(selectedProject => selectedProject.Equals(component.Project)) ?? false);
        public IEnumerable<TypeModel> IncludedTypes => includedTypes.Where(component => SelectedProjects?.Any(selectedProject => selectedProject.Equals(component.Project)) ?? false);
        public IEnumerable<TypeModel> ExcludedTypes => includedTypes.Where(component => SelectedProjects?.Any(selectedProject => selectedProject.Equals(component.Project)) ?? false);
        public IEnumerable<StatusModel> IncludedStatues => includedStatues.Where(component => SelectedProjects?.Any(selectedProject => selectedProject.Equals(component.Project)) ?? false);
        public IEnumerable<StatusModel> ExcludedStatues => includedStatues.Where(component => SelectedProjects?.Any(selectedProject => selectedProject.Equals(component.Project)) ?? false);

        public List<ProjectModel>? SelectedProjects { get; set; }
        public List<ComponentModel>? SelectedIncludedComponents { get; set; }
        public List<ComponentModel>? SelectedExcludedComponents { get; set; }
        public List<TypeModel>? SelectedIncludedTypes { get; set; }
        public List<TypeModel>? SelectedExcludedTypes { get; set; }
        public List<StatusModel>? SelectedIncludedStatues { get; set; }
        public List<StatusModel>? SelectedExcludedStatues { get; set; }

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
                .Where(item => !includedTypes.Concat(excludedTypes).Select(type => type.Id).Contains(item.IncludedTypeModel.Id))
                .Select(item => item.IncludedTypeModel)
                .DistinctBy(status => status.Id)
                .ToList();

            var excludedTypesModel = types
                .Where(item => !includedTypes.Concat(excludedTypes).Select(type => type.Id).Contains(item.ExcludedTypeModel.Id))
                .Select(item => item.ExcludedTypeModel)
                .DistinctBy(status => status.Id)
                .ToList();

            includedTypes = includedTypes.Concat(includedTypesModel).OrderBy(type => type.Name).ToList();
            excludedTypes = excludedTypes.Concat(excludedTypesModel).OrderBy(type => type.Name).ToList();

            var includedStatusesModel = types
                .Where(status => !includedStatues.Concat(excludedStatues).Select(status => status.Id).Contains(status.IncludedStatusModel.Id))
                .Select(item => item.IncludedStatusModel)
                .DistinctBy(status => status.Id)
                .ToList();

            var excludedStatusesModel = types
                .Where(status => !includedStatues.Concat(excludedStatues).Select(status => status.Id).Contains(status.ExcludedStatusModel.Id))
                .Select(item => item.ExcludedStatusModel)
                .DistinctBy(status => status.Id)
                .ToList();

            includedStatues = includedStatues.Concat(includedStatusesModel).OrderBy(status => status.Name).ToList();
            excludedStatues = excludedStatues.Concat(excludedStatusesModel).OrderBy(status => status.Name).ToList();
        }
    }
}