namespace Kira.Builders;

using System.Text;

public class FilterBuilder
{
    public string BuildJqlQuery(IEnumerable<string>? selectedProjects, IEnumerable<(string ProjectId, string ComponentId)> selectedIncludedComponents, IEnumerable<(string ProjectId, string ComponentId)> selectedExcludedComponents)
    {
        var projects = selectedProjects?.ToList() ?? new();
        var includedComponents = selectedIncludedComponents.ToList();
        var excludedComponents = selectedExcludedComponents.ToList();

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

        return jqlQueryBuilder.ToString();
    }
}