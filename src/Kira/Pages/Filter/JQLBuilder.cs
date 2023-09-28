namespace Kira.Pages.Filter;

using System.Text;

public static class JqlBuilder
{
    public static string ToJql(this FilterForm.Designer model) => InternalToJql(model);

    static string InternalToJql(this JqlModel model)
    {
        var jqlQueryBuilder = new StringBuilder();

        // Add the "project in ()" part to the query
        if (model.Projects.Any()) jqlQueryBuilder.Append($"project in ({string.Join(',', model.Projects)})");

        // Check if there are included components
        if (model.IncludedComponents.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"component in ({string.Join(',', model.IncludedComponents.Select(c => c.Id))})");
        }

        // Check if there are excluded components
        if (model.ExcludedComponents.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"component not in ({string.Join(',', model.ExcludedComponents.Select(c => c.Id))})");
        }

        // Check if there are included types
        if (model.IncludedTypes.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"type in ({string.Join(',', model.IncludedTypes.Select(t => t.Id))})");
        }

        // Check if there are excluded types
        if (model.ExcludedTypes.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"type not in ({string.Join(',', model.ExcludedTypes.Select(t => t.Id))})");
        }

        // Check if there are included statuses
        if (model.IncludedStatues.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"status in ({string.Join(',', model.IncludedStatues.Select(s => s.Id))})");
        }

        // Check if there are excluded statuses
        if (model.ExcludedStatues.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"status not in ({string.Join(',', model.ExcludedStatues.Select(s => s.Id))})");
        }

        return jqlQueryBuilder.ToString();
    }

    static IEnumerable<(string ProjectId, string Id)> ToTuple(this IEnumerable<FilterForm.IFilterModel>? model) => model?.Select(m => (m.Project.Id, m.Id)) ?? Enumerable.Empty<(string, string)>();

    public class JqlModel(IEnumerable<string>? projects, IEnumerable<(string ProjectId, string Id)>? includedComponents, IEnumerable<(string ProjectId, string Id)>? excludedComponents, IEnumerable<(string ProjectId, string Id)>? includedTypes, IEnumerable<(string ProjectId, string Id)>? excludedTypes, IEnumerable<(string ProjectId, string Id)>? includedStatues, IEnumerable<(string ProjectId, string Id)>? excludedStatues)
    {
        public IEnumerable<string> Projects => projects ?? Enumerable.Empty<string>();

        public IEnumerable<(string ProjectId, string Id)> IncludedComponents => includedComponents ?? Enumerable.Empty<(string ProjectId, string Id)>();
        public IEnumerable<(string ProjectId, string Id)> ExcludedComponents => excludedComponents ?? Enumerable.Empty<(string ProjectId, string Id)>();

        public IEnumerable<(string ProjectId, string Id)> IncludedTypes => includedTypes ?? Enumerable.Empty<(string ProjectId, string Id)>();
        public IEnumerable<(string ProjectId, string Id)> ExcludedTypes => excludedTypes ?? Enumerable.Empty<(string ProjectId, string Id)>();

        public IEnumerable<(string ProjectId, string Id)> IncludedStatues => includedStatues ?? Enumerable.Empty<(string ProjectId, string Id)>();
        public IEnumerable<(string ProjectId, string Id)> ExcludedStatues => excludedStatues ?? Enumerable.Empty<(string ProjectId, string Id)>();

        public static implicit operator JqlModel(FilterForm.Designer formModel) =>
            new(
                formModel.Projects.Select(project => project.Id),
                formModel.IncludedComponents.ToTuple(),
                formModel.ExcludedComponents.ToTuple(),
                formModel.IncludedTypes.ToTuple(),
                formModel.ExcludedTypes.ToTuple(),
                formModel.IncludedStatues.ToTuple(),
                formModel.ExcludedStatues.ToTuple()
            );
    }
}