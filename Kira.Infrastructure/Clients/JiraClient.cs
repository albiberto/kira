namespace Kira.Infrastructure.Clients;

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;
using Microsoft.Extensions.Logging;

public class JiraClient(HttpClient http, ILogger<JiraClient> logger)
{
    static JsonSerializerOptions JsonSerializerOptions =>
        new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    public async Task<IEnumerable<Component>> GetAllProjectsComponents(string projectId)
    {
        try
        {
            var result = await http.GetAsync($"project/{projectId}/components");
            var json = await result.Content.ReadAsStringAsync();
            var stream = await result.Content.ReadAsStreamAsync();

            var components = await JsonSerializer.DeserializeAsync<IEnumerable<Component>>(stream, JsonSerializerOptions);
            if (components is not null) return components;
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Error during JQL query execution.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error occurred.");
        }

        return Enumerable.Empty<Component>();
    }

    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        try
        {
            var result = await http.GetAsync("project");

            Console.WriteLine(await result.Content.ReadAsStringAsync());

            var stream = await result.Content.ReadAsStreamAsync();

            var projects = await JsonSerializer.DeserializeAsync<IEnumerable<Project>>(stream, JsonSerializerOptions);
            if (projects is not null) return projects;
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Error during JQL query execution.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error occurred.");
        }

        return Enumerable.Empty<Project>();
    }

    public async Task<ICollection<JiraIssues>> PostSearchAsync(IEnumerable<Project> projects, string? query = default, IEnumerable<string>? fields = default, CancellationToken cancellationToken = default)
    {
        var jql = QueryBuilder(projects.ToArray(), query);

        var request = new
        {
            jql,
            startAt = 0,
            maxResults = 100,
            fields = fields?.ToArray() ?? Array.Empty<string>()
        };

        var result = await http.PostAsJsonAsync("search", request, cancellationToken);

        if (!result.IsSuccessStatusCode) return Array.Empty<JiraIssues>();

        await using (var stream = await result.Content.ReadAsStreamAsync(cancellationToken))
            try
            {
                var issues = await JsonSerializer.DeserializeAsync<ICollection<JiraIssues>>(stream, JsonSerializerOptions, cancellationToken);
                if (issues is not null) return issues;
            }
            catch (JsonException e)
            {
                logger.LogError(e, "Error during JQL query execution: {jql}", jql);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An unexpected error occurred.");
            }

        return Array.Empty<JiraIssues>();
    }

    static string QueryBuilder(ICollection<Project> projects, string? query)
    {
        var jql = new StringBuilder();

        var withProjects = projects.Any();
        var withQuery = !string.IsNullOrEmpty(query);

        if (withProjects)
        {
            jql.Append("project in (");
            jql.AppendJoin(',', projects.Select(project => project.Key));
            jql.Append(')');
        }

        if (withQuery && withProjects) jql.Append(" AND ");

        if (!string.IsNullOrEmpty(query))
        {
            jql.Append(" AND ");
            jql.Append(query);
        }

        jql.Append(' ');
        jql.Append("ORDER BY key ASC");

        return jql.ToString();
    }
}