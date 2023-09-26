namespace Kira.Extensions;

using Pages;
using Radzen;

public static class JqlExtensions
{
    public static IEnumerable<(string ProjectId, string Id)> ToTuple(this IEnumerable<Filter.IFilterModel>? model) => model?.Select(m => (m.Project.Id, m.Id)) ?? Enumerable.Empty<(string, string)>();

    public static IEnumerable<(string Field, string Order)> ToOrderClause(this LoadDataArgs args, string @default)
    {
        return args.Sorts is null
            ? new[] { (@default, "DESC") }
            : args.Sorts.Select(sort => (sort.Property.Contains(' ') ? $@"""{sort.Property}""" : sort.Property, sort.SortOrder == SortOrder.Ascending ? "ASC" : "DESC"));
    }
}