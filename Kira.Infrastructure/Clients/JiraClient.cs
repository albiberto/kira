namespace Kira.Infrastructure.Clients;

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
}