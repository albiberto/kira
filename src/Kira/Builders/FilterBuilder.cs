namespace Kira.Builders;

using System.Text;

public class FilterBuilder
{
    public string BuildJqlQuery(
        IEnumerable<string>? selectedProjects,
        IEnumerable<(string ProjectId, string ComponentId)> selectedIncludedComponents,
        IEnumerable<(string ProjectId, string ComponentId)> selectedExcludedComponents,
        IEnumerable<(string ProjectId, string Id)> selectedIncludedTypes,
        IEnumerable<(string ProjectId, string Id)> selectedExcludedTypes,
        IEnumerable<(string ProjectId, string Id)> selectedIncludedStatus,
        IEnumerable<(string ProjectId, string Id)> selectedExcludedStatus)
    {
        var projects = selectedProjects?.ToList() ?? new();
        var includedComponents = selectedIncludedComponents.ToList();
        var excludedComponents = selectedExcludedComponents.ToList();
        var includedTypes = selectedIncludedTypes.ToList();
        var excludedTypes = selectedExcludedTypes.ToList();
        var includedStatus = selectedIncludedStatus.ToList();
        var excludedStatus = selectedExcludedStatus.ToList();

        var jqlQueryBuilder = new StringBuilder();

        // Add the "project in ()" part to the query
        if (projects.Any()) jqlQueryBuilder.Append($"project in ({string.Join(", ", projects)})");

        // Check if there are included components
        if (includedComponents.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"component in ({string.Join(", ", includedComponents.Select(c => c.ComponentId))})");
        }

        // Check if there are excluded components
        if (excludedComponents.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"component not in ({string.Join(", ", excludedComponents.Select(c => c.ComponentId))})");
        }

        // Check if there are included types
        if (includedTypes.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"type in ({string.Join(", ", includedTypes.Select(t => t.Id))})");
        }

        // Check if there are excluded types
        if (excludedTypes.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"type not in ({string.Join(", ", excludedTypes.Select(t => t.Id))})");
        }

        // Check if there are included statuses
        if (includedStatus.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"status in ({string.Join(", ", includedStatus.Select(s => s.Id))})");
        }

        // Check if there are excluded statuses
        if (excludedStatus.Any())
        {
            if (jqlQueryBuilder.Length > 0) jqlQueryBuilder.Append(" AND ");
            jqlQueryBuilder.Append($"status not in ({string.Join(", ", excludedStatus.Select(s => s.Id))})");
        }

        return jqlQueryBuilder.ToString();
    }
}