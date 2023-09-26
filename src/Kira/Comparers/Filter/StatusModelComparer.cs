namespace Kira.Comparers.Filter;

using Pages;

public class StatusModelComparer : IEqualityComparer<Filter.StatusModel>
{
    public bool Equals(Filter.StatusModel? x, Filter.StatusModel? y) => string.Equals(x?.Id, y?.Id, StringComparison.InvariantCultureIgnoreCase);

    public int GetHashCode(Filter.StatusModel obj) => obj.Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
}