namespace Kira.Builders;

using System.Text;

public static class JqlBuilder
{
    public static string ToJql(this JqlModel model)
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

        // Add the sorting clause if it's present
        if (model.Order.Any())
        {
            jqlQueryBuilder.Append(" ORDER BY ");
            jqlQueryBuilder.Append(string.Join(", ", model.Order.Select(order => $"{order.Field} {order.Order}")));
        }

        return jqlQueryBuilder.ToString();
    }
}