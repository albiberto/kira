namespace Kira.Pages;

using Domain;

public partial class FilterForm
{
    public interface IFilterModel
    {
        ProjectModel Project { get; }
        string Id { get; }
        bool Disabled { get; }

        void Disable(bool disable);
    }

    public record ProjectModel(Project project)
    {
        public string Id { get; } = project.Id;
        public string Key { get; } = project.Key;
        public string Avatar { get; } = project.AvatarUrls._16x16;
    }

    public record ComponentModel(ProjectModel project, Component component) : IFilterModel
    {
        public string Name { get; } = $"{project.Key} | {component.Name}";
        public ProjectModel Project { get; } = project;

        public string Id { get; } = component.Id;

        public bool Disabled { get; private set; }

        public void Disable(bool disable) => Disabled = disable;
    }

    public record TypeModel(ProjectModel project, ProjectType type) : IFilterModel
    {
        public string Name { get; } = type.Name;
        public ProjectModel Project { get; } = project;

        public string Id { get; } = type.Id;

        public bool Disabled { get; private set; }

        public void Disable(bool disable) => Disabled = disable;
    }

    public record StatusModel(ProjectModel project, ProjectType type, Status status) : IFilterModel
    {
        public ProjectType Type { get; } = type;
        public string Name { get; } = status.Name;
        public ProjectModel Project { get; } = project;

        public string Id { get; } = status.Id;

        public bool Disabled { get; private set; }

        public void Disable(bool disable) => Disabled = disable;
    }
}