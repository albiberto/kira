namespace Kira.Infrastructure.Clients;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;
using Microsoft.Extensions.Logging;

public abstract class JiraClient(HttpClient http, ILogger<JiraClient> logger)
{
    public class Customer(IHttpClientFactory factory, ILogger<JiraClient> logger) : JiraClient(factory.CreateClient("Customer"), logger);
    public class My(IHttpClientFactory factory, ILogger<JiraClient> logger) : JiraClient(factory.CreateClient("My"), logger);

    static JsonSerializerOptions JsonSerializerOptions
    {
        get
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            // options.Converters.Add(new DateOnlyConverter());

            return options;
        }
    }

    public async Task<IEnumerable<Project>> GetAllProjects()
    {
        try
        {
            var result = await http.GetAsync("project");

            Console.WriteLine(await result.Content.ReadAsStringAsync());

            var json = await result.Content.ReadAsStringAsync();
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

    public async Task<IEnumerable<ProjectType>> GetAllProjectStatues(string projectId)
    {
        try
        {
            var result = await http.GetAsync($"project/{projectId}/statuses");

            Console.WriteLine(await result.Content.ReadAsStringAsync());

            var stream = await result.Content.ReadAsStreamAsync();
            var json = await result.Content.ReadAsStringAsync();

            var statues = await JsonSerializer.DeserializeAsync<IEnumerable<ProjectType>>(stream, JsonSerializerOptions);
            if (statues is not null) return statues;
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Error during JQL query execution.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error occurred.");
        }

        return Enumerable.Empty<ProjectType>();
    }

    public async Task<IEnumerable<Issue>> PostSearchAsync(string jql, IEnumerable<string> fields)
    {
        var request = new
        {
            jql,
            startAt = 0,
            maxResults = 100,
            fields = fields.ToArray()
        };

        var result = await http.PostAsJsonAsync("search", request);
        var json = await result.Content.ReadAsStringAsync();

        if (!result.IsSuccessStatusCode) return Array.Empty<Issue>();

        await using (var stream = await result.Content.ReadAsStreamAsync())
            try
            {
                var issues = await JsonSerializer.DeserializeAsync<SearchResponse>(stream, JsonSerializerOptions);
                if (issues is not null) return issues.Issues;
            }
            catch (JsonException e)
            {
                logger.LogError(e, "Error during JQL query execution: {jql}", jql);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An unexpected error occurred.");
            }

        return Array.Empty<Issue>();
    }
}