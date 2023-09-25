namespace Kira.Comparers.Filter;

using Pages;

public class TypeModelComparer : IEqualityComparer<Filter.TypeModel>
{
    public bool Equals(Filter.TypeModel? x, Filter.TypeModel? y) => string.Equals(x?.Id, y?.Id, StringComparison.InvariantCultureIgnoreCase);

    public int GetHashCode(Filter.TypeModel obj) => obj.Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
}