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
}