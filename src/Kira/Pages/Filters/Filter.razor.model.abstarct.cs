namespace Kira.Pages.Filters;

public partial class Filter
{
    public interface IFilterModel
    {
        ProjectModel Project { get; }
        string Id { get; }
        string Name { get; }

        bool Disabled { get; }
        void Disable(bool disable);
    }
}