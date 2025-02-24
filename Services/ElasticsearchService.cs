using Elasticsearch.Net;
using Nest;
using System;
using System.Threading.Tasks;

public class ElasticsearchService
{
    private readonly ElasticClient _client;

    public ElasticsearchService(IConfiguration configuration)
    {
        var settings = new ConnectionSettings(new Uri(configuration["Elasticsearch:Uri"]))
            .DefaultIndex("tasks") // Default index
            .DisableDirectStreaming();

        _client = new ElasticClient(settings);
    }

    /// <summary>
    /// Index a new task in Elasticsearch.
    /// </summary>
    public async Task IndexTaskAsync(TaskItem task)
    {
        await _client.IndexDocumentAsync(task);
    }

    /// <summary>
    /// Search tasks by title.
    /// </summary>
    public async Task<ISearchResponse<TaskItem>> SearchTasksAsync(string query)
    {
        return await _client.SearchAsync<TaskItem>(s => s
            .Query(q => q.Match(m => m.Field(f => f.Title).Query(query))));
    }
}
