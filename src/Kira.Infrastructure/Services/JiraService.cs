namespace Kira.Infrastructure.Services;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;
using Microsoft.Extensions.Logging;

public class JiraService(HttpClient http, ILogger<JiraService> logger)
{
    static JsonSerializerOptions JsonSerializerOptions =>
        new()
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    public Task<IEnumerable<Project>> GetAllProjects() => Get<Project>("project");
    public Task<IEnumerable<Component>> GetAllProjectsComponents(string projectId) => Get<Component>($"project/{projectId}/components");
    public Task<IEnumerable<ProjectType>> GetAllProjectStatues(string projectId) => Get<ProjectType>($"project/{projectId}/statuses");

    public async Task<IEnumerable<Issue>> PostSearchAsync(string jql, IEnumerable<string> fields, int startAt = 0, int maxResults = 100)
    {
        var request = new
        {
            jql,
            startAt = 0,
            maxResults = 100,
            fields = fields.ToArray()
        };

        var response = await http.PostAsJsonAsync("search", request);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Status Code: {0}. Error: {1}", response.StatusCode, response.ReasonPhrase);
            return Array.Empty<Issue>();
        }

        await using (var stream = await response.Content.ReadAsStreamAsync())
            try
            {
                var result = await JsonSerializer.DeserializeAsync<SearchResponse>(stream, JsonSerializerOptions);
                if (result is not null) return result.Issues;
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
    
    async Task<IEnumerable<T>> Get<T>(string uri)
    {
        var response = await http.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Status Code: {0}. Error: {1}", response.StatusCode, response.ReasonPhrase);
            return Array.Empty<T>();
        }
        
        logger.LogDebug(await response.Content.ReadAsStringAsync());

        await using (var stream = await response.Content.ReadAsStreamAsync())
            try
            {
                var result = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(stream, JsonSerializerOptions);
                if (result is not null) return result;
            }
            catch (JsonException e)
            {
                logger.LogError(e, "Error during JQL query execution: {jql}", uri);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An unexpected error occurred.");
            }

        return Array.Empty<T>();
    }
}