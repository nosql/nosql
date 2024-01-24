using NoSql.ArangoDb.Storage;
using NoSql.Scaffolding;
using NoSql.Storage;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoSql.ArangoDb.Scaffolding;

public class ArangoDatabaseFactory(ArangoDbConnection connection) : IDatabaseFactory
{
    public TableCreateResult CreateTable(NoSqlTypeInfo table) => CreateTableAsync(table).Result;

    public bool DropTable(string tableName) => DropTableAsync(tableName).Result;

    public async Task<TableCreateResult> CreateTableAsync(NoSqlTypeInfo table, CancellationToken cancellationToken = default)
    {
        var collection = await GetCollectionMetadataAsync(table.Name);
        if (collection == null)
        {
            await CreateCollectionAsync(table.Name, ArangoCollectionType.Document);
            return TableCreateResult.Created;
        }
        return TableCreateResult.None;
    }

    public async Task<bool> DropTableAsync(string tableName, CancellationToken cancellationToken = default)
    {
        var response = await connection.CreateHttpClient().DeleteAsync($"collection/{tableName}", cancellationToken);

        //400: If the collection - name is missing, then a HTTP 400 is returned.
        //404: If the collection - name is unknown, then a HTTP 404 is returned.

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new HttpRequestException(response.StatusCode.ToString());
        }
    }

    public virtual async Task CreateCollectionAsync(string collectionName, ArangoCollectionType type = ArangoCollectionType.Document)
    {
        var request = new StringContent($"{{\"name\":\"{collectionName}\",\"type\":{(int)type}}}");
        var response = await connection.CreateHttpClient().PostAsync("collection", request);

        //400: If the collection - name is missing, then a HTTP 400 is returned.
        //404: If the collection - name is unknown, then a HTTP 404 is returned.
        //409
        if (response.IsSuccessStatusCode)
        {
            return;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Conflict)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new HttpRequestException(response.StatusCode.ToString());
        }
    }

    public virtual async Task<ArangoCollectionInfo?> GetCollectionMetadataAsync(string collectionName)
    {
        var response = await connection.CreateHttpClient().GetAsync($"collection/{collectionName}");

        //404: If the collection-name is unknown, then a HTTP 404 is returned.
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ArangoCollectionMetadataResult>(content)!;
            if (result.Error)
                throw new ArangoException(result);

            return new ArangoCollectionInfo
            {
                Id = result.Id,
                IsSystem = result.IsSystem,
                Name = result.Name,
                GloballyUniqueId = result.GloballyUniqueId,
                Status = result.Status,
                Type = result.Type
            };
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        else
        {
            throw new HttpRequestException(response.StatusCode.ToString());
        }
    }

    private class ArangoCollectionMetadataResult : ArangoResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("isSystem")]
        public bool IsSystem { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("globallyUniqueId")]
        public string GloballyUniqueId { get; set; } = null!;

        [JsonPropertyName("type")]
        public ArangoCollectionType Type { get; set; }

        [JsonPropertyName("status")]
        public ArangoCollectionStatus Status { get; set; }
    }
}
