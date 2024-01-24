using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NoSql.ArangoDb;

internal static class ArangoHttpClientExtensions
{
    public static async Task CursorAsync(this HttpClient httpClient, string aql, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(aql))
            throw new ArgumentNullException(nameof(aql));

        var query = new JsonObject() { { "query", aql } };
        var request = new StringContent(query.ToJsonString());

        var response = await httpClient.PostAsync("cursor", request, cancellationToken);

        //201 is returned if the result set can be created by the server.
        //400 is returned if the JSON representation is malformed or the query specification is missing from the request.
        //404: The server will respond with HTTP 404 in case a non-existing collection is accessed in the query.
        //405: The server will respond with HTTP 405 if an unsupported HTTP method is used.
        if (response.IsSuccessStatusCode)
        {
            return;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.MethodNotAllowed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }
    }

    public static async Task<ArangoQueryList<T>> CursorNextAsync<T>(this HttpClient httpClient, string id, JsonSerializerOptions serializerOptions, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentNullException(nameof(id));

        var response = await httpClient.PostAsync($"cursor/{id}", null, cancellationToken);

        //201 is returned if the result set can be created by the server.
        //400 is returned if the JSON representation is malformed or the query specification is missing from the request.
        //404: The server will respond with HTTP 404 in case a non-existing collection is accessed in the query.
        //405: The server will respond with HTTP 405 if an unsupported HTTP method is used.
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ArangoQueryList<T>>(content, serializerOptions)!;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.MethodNotAllowed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }
    }

    public static async Task<T?> FindOneOrDefaultAsync<T>(this HttpClient httpClient, string aql, JsonSerializerOptions serializerOptions, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(aql))
            throw new ArgumentNullException(nameof(aql));

        var query = new JsonObject
        {
            { "query", aql },
            { "batchSize", 1 },
            { "count", false }
        };
        var request = new StringContent(query.ToJsonString());
        var response = await httpClient.PostAsync("cursor", request, cancellationToken);

        //201 is returned if the result set can be created by the server.
        //400 is returned if the JSON representation is malformed or the query specification is missing from the request.
        //404: The server will respond with HTTP 404 in case a non-existing collection is accessed in the query.
        //405: The server will respond with HTTP 405 if an unsupported HTTP method is used.
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ArangoQueryList<JsonElement>>(content, serializerOptions)!;
            if (result.Result.Count == 0)
                return default;

            var value = result.Result[0];
            if (value.ValueKind == JsonValueKind.Null)
                return default;

            return JsonSerializer.Deserialize<T>(value, serializerOptions);
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.MethodNotAllowed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }

    }

    public static async Task<ArangoQueryList<T>> CursorAsync<T>(this HttpClient httpClient, string aql, AqlQueryOptions? options, JsonSerializerOptions serializerOptions, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(aql))
            throw new ArgumentNullException(nameof(aql));

        options ??= AqlQueryOptions.Default;

        var query = new JsonObject
        {
            { "query", aql },
            { "batchSize", options.BatchSize },
            { "count", options.Count }
        };
        var request = new StringContent(query.ToJsonString());
        var response = await httpClient.PostAsync("cursor", request, cancellationToken);

        //201 is returned if the result set can be created by the server.
        //400 is returned if the JSON representation is malformed or the query specification is missing from the request.
        //404: The server will respond with HTTP 404 in case a non-existing collection is accessed in the query.
        //405: The server will respond with HTTP 405 if an unsupported HTTP method is used.
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ArangoQueryList<T>>(content, serializerOptions)!;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.MethodNotAllowed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }
    }

    public static async Task<T?> GetDocumentAsync<T>(this HttpClient httpClient, string collectionName, string key, JsonSerializerOptions serializerOptions, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentNullException(nameof(collectionName));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var response = await httpClient.GetAsync($"document/{collectionName}/{key}", cancellationToken);

        //200: is returned if the document was found
        //304: is returned if the “If - None - Match” header is given and the document has the same version
        //404: is returned if the document or collection was not found
        //412: is returned if an “If - Match” header is given and the found document has a different version.The response will also contain the found document’s current revision in the _rev attribute.Additionally, the attributes _id and _key will be returned.

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, serializerOptions);
        }
        else if (response.StatusCode == HttpStatusCode.NotModified)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, serializerOptions);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.PreconditionFailed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }
    }

    public static async Task<ArangoKey[]> PostDocumentAsync<T>(this HttpClient httpClient,
        string collectionName,
        IEnumerable<T> documents,
        ArangoDocumentCreateOptions? createOptions,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentNullException(nameof(collectionName));

        if (documents == null)
            throw new ArgumentNullException(nameof(documents));

        if (!documents.Any())
            throw new ArgumentException($"文档集合中至少包含一个元素");

        var url = $"document/{collectionName}";

        createOptions ??= ArangoDocumentCreateOptions.Default;

        var parameters = createOptions.GetParameters();
        if (parameters.Count > 0)
        {
            url += "?" + string.Join('&', parameters.Select(x => x.Key + "=" + x.Value));
        }

        var json = JsonSerializer.SerializeToNode(documents, serializerOptions)!;
        if (json is JsonArray array)
        {
            foreach (var item in array)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (item is JsonObject obj && obj.ContainsKey("_key") && obj["_key"] == null)
                    obj.Remove("_key");
            }
        }

        var response = await httpClient.PostAsync(url, new StringContent(json.ToJsonString(serializerOptions)), cancellationToken);

        //201: is returned if waitForSync was true and operations were processed.
        //202: is returned if waitForSync was false and operations were processed.
        //400: is returned if the body does not contain a valid JSON representation of an array of documents.The response body contains an error document in this case.
        //404: is returned if the collection specified by collection is unknown.The response body contains an error document in this case.

        if (response.IsSuccessStatusCode)
        {
            if (createOptions.Silent)
                return Array.Empty<ArangoKey>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ArangoKey[]>(content)!;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }
    }

    public static async Task<ArangoKey> PostDocumentAsync(this HttpClient httpClient,
        string collectionName,
        object document,
        ArangoDocumentCreateOptions? createOptions,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentNullException(nameof(collectionName));

        if (document == null)
            throw new ArgumentNullException(nameof(document));

        var url = $"document/{collectionName}";

        createOptions ??= ArangoDocumentCreateOptions.Default;

        var parameters = createOptions.GetParameters();
        if (parameters.Count > 0)
        {
            url += "?" + string.Join('&', parameters.Select(x => x.Key + "=" + x.Value));
        }

        var json = JsonSerializer.SerializeToNode(document, serializerOptions)!;
        if (json is JsonObject obj && obj.ContainsKey("_key") && obj["_key"] == null)
        {
            obj.Remove("_key");
        }

        var response = await httpClient.PostAsync(url, new StringContent(json.ToJsonString(serializerOptions)), cancellationToken);

        //201: is returned if the documents were created successfully and waitForSync was true.
        //202: is returned if the documents were created successfully and waitForSync was false.
        //400: is returned if the body does not contain a valid JSON representation of one document.The response body contains an error document in this case.
        //404: is returned if the collection specified by collection is unknown.The response body contains an error document in this case.
        //409: is returned in the single document case if a document with the same qualifiers in an indexed attribute conflicts with an already existing document and thus violates that unique constraint.The response body contains an error document in this case.
        if (response.IsSuccessStatusCode)
        {
            if (createOptions.Silent)
                return default;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ArangoKey>(content);
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Forbidden)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }

    }

    public static async Task<ArangoKey> PutDocumentAsync(this HttpClient httpClient, string collectionName, string key, object document, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentNullException(nameof(collectionName));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        if (document == null)
            throw new ArgumentNullException(nameof(document));

        var json = JsonSerializer.Serialize(document, options);
        var response = await httpClient.PutAsync($"document/{collectionName}/{key}", new StringContent(json), cancellationToken);

        //201: is returned if the document was replaced successfully and waitForSync was true.
        //202: is returned if the document was replaced successfully and waitForSync was false.
        //400: is returned if the body does not contain a valid JSON representation of a document.The response body contains an error document in this case.
        //404: is returned if the collection or the document was not found.
        //412: is returned if the precondition was violated.The response will also contain the found documents’ current revisions in the _rev attributes.Additionally, the attributes _id and _key will be returned.
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ArangoKey>(content);
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.PreconditionFailed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }

    }

    public static async Task<ArangoKey> PatchDocumentAsync(this HttpClient httpClient, string collectionName, string key, IDictionary<string, object?> document,
        ArangoDocumentUpdateOptions? updateOptions,
        JsonSerializerOptions serializerOptions,
        CancellationToken cancellationToken)
    {
        var url = $"document/{collectionName}/{key}";
        if (updateOptions != null)
        {
            var parameters = updateOptions.GetParameters();
            if (parameters.Count > 0)
            {
                url += "?" + string.Join('&', parameters.Select(x => x.Key + "=" + x.Value));
            }
        }

        var json = JsonSerializer.Serialize(document, serializerOptions);

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
        {
            Content = new StringContent(json)
        };
        var response = await httpClient.SendAsync(request, cancellationToken);

        //201: is returned if the document was updated successfully and waitForSync was true.
        //202: is returned if the document was updated successfully and waitForSync was false.
        //400: is returned if the body does not contain a valid JSON representation of a document.The response body contains an error document in this case.
        //404: is returned if the collection or the document was not found.
        //412: is returned if the precondition was violated.The response will also contain the found documents’ current revisions in the _rev attributes.Additionally, the attributes _id and _key will be returned.
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ArangoKey>(content);
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.PreconditionFailed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }

    }

    public static async Task<ArangoKey> DeleteDocumentAsync(this HttpClient httpClient, string collectionName, string key, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(collectionName))
            throw new ArgumentNullException(nameof(collectionName));

        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key));

        var response = await httpClient.DeleteAsync($"document/{collectionName}/{key}", cancellationToken);

        //200: is returned if the document was removed successfully and waitForSync was true.
        //202: is returned if the document was removed successfully and waitForSync was false.
        //404: is returned if the collection or the document was not found.The response body contains an error document in this case.
        //412: is returned if a “If - Match” header or rev is given and the found document has a different version.The response will also contain the found document’s current revision in the _rev attribute.Additionally, the attributes _id and _key will be returned.
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ArangoKey>(content);
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.PreconditionFailed)
        {
            var content = await response.Content.ReadAsStringAsync();
            var error = JsonSerializer.Deserialize<ArangoResult>(content);
            throw new ArangoException(error);
        }
        else
        {
            throw new ArangoException((int)response.StatusCode);
        }

    }
}
