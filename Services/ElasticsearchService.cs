using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskPlannerAPI.Models;  // Ensure this matches your actual namespace

public class ElasticsearchService
{
    private readonly IElasticClient _elasticClient;

    public ElasticsearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    /// <summary>
    /// Indexes a TaskItem in Elasticsearch.
    /// </summary>
    /// <param name="task">The task to be indexed.</param>
    /// <returns>True if indexing was successful, otherwise false.</returns>
    public async Task<bool> IndexTaskAsync(TaskItem task)
    {
        var response = await _elasticClient.IndexDocumentAsync(task);
        return response.IsValid;
    }

    /// <summary>
    /// Searches tasks based on title or description.
    /// </summary>
    /// <param name="query">Search term.</param>
    /// <returns>List of matching tasks.</returns>
    public async Task<IEnumerable<TaskItem>> SearchTasksAsync(string query)
    {
        var response = await _elasticClient.SearchAsync<TaskItem>(s => s
            .Query(q => q
                .Bool(b => b
                    .Should(
                        m => m.Match(match => match.Field(f => f.Title).Query(query)),
                        m => m.Match(match => match.Field(f => f.Description).Query(query))
                    )
                )
            )
        );

        return response.Documents;
    }
}