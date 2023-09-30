namespace Kira.Pages;

using Domain;

public partial class Filter
{
    public record ProjectModel(Project Project)
    {
        public string Id { get; } = Project.Id;
        public string Key { get; } = Project.Key;
        public string Avatar { get; } = Project.AvatarUrls._16x16;
    }

    public record ComponentModel(ProjectModel Project, Component Component) : IFilterModel
    {
        public ProjectModel Project { get; } = Project;

        public string Name { get; } = $"{Project.Key} | {Component.Name}";
        public string Id { get; } = Component.Id;

        public bool Disabled { get; private set; }
        public void Disable(bool disable) => Disabled = disable;
    }

    public record TypeModel(ProjectModel Project, ProjectType Type) : IFilterModel
    {
        public ProjectModel Project { get; } = Project;

        public string Id { get; } = Type.Id;
        public string Name { get; } = Type.Name;


        public bool Disabled { get; private set; }
        public void Disable(bool disable) => Disabled = disable;
    }

    public record StatusModel(ProjectModel Project, ProjectType Type, Status Status) : IFilterModel
    {
        public ProjectModel Project { get; } = Project;

        public string Id { get; } = Status.Id;
        public string Name { get; } = Status.Name;


        public bool Disabled { get; private set; }
        public void Disable(bool disable) => Disabled = disable;
    }
}