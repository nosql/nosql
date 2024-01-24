using System.Text.Json.Serialization;

namespace NoSql.ArangoDb;

/// <summary>
/// Arango文档健
/// </summary>
public readonly struct ArangoKey
{
    [JsonConstructor]
    public ArangoKey(string id, string key, string rev, string? oldRev = null)
    {
        Id = id;
        Key = key;
        Rev = rev;
        OldRev = oldRev;
    }

    /// <summary>
    /// Id，ArangoDB的文档Id,由Collection名称加文档Key构成：{collection}/{key}
    /// </summary>
    [JsonPropertyName("_id")]
    public string Id { get; }

    /// <summary>
    /// Key，不可重复建
    /// </summary>
    [JsonPropertyName("_key")]
    public string Key { get; }

    /// <summary>
    /// Rev
    /// </summary>
    [JsonPropertyName("_rev")]
    public string Rev { get; }

    /// <summary>
    /// 当替换文档时，该属性为原文的的Rev值
    /// </summary>
    [JsonPropertyName("_oldRev")]
    public readonly string? OldRev { get; }
}
