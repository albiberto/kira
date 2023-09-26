namespace Kira.Pages;

using Domain;

public partial class Filter
{
    public class ProjectModel(Project project)
    {
        public string Id { get; } = project.Id;
        public string Key { get; } = project.Key;
        public string Avatar { get; } = project.AvatarUrls._16x16;
    }

    public class ComponentModel(ProjectModel project, Component component, bool disabled = default)
    {
        public string ProjectId { get; } = project.Id;

        public string Id { get; } = component.Id;
        public string Name { get; } = $"{project.Key} | {component.Name}";
        public bool Disabled { get; private set; } = disabled;

        public bool Disable(bool disable) => Disabled = disable;
    }

    public class TypeModel(ProjectModel project, ProjectType type, bool disabled = default)
    {
        public string ProjectId { get; } = project.Id;

        public string Id { get; } = type.Id;
        public string Name { get; } = type.Name;

        public bool Disabled { get; private set; } = disabled;

        public bool Disable(bool disable) => Disabled = disable;
    }

    public class StatusModel(ProjectModel project, ProjectType type, Status status, bool disabled = default)
    {
        public string ProjectId { get; } = project.Id;
        public string TypeId { get; } = type.Id;

        public string Id { get; } = status.Id;
        public string Name { get; } = status.Name;
        public bool Disabled { get; private set; } = disabled;

        public bool Disable(bool disable) => Disabled = disable;
    }
}