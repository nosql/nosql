using System.Text.Json.Serialization;

namespace NoSql.ArangoDb;

/// <summary>
/// 查询返回值
/// </summary>
/// <typeparam name="T"></typeparam>
public class ArangoQueryList<T>
{
    /// <summary>
    /// 查询结果
    /// </summary>
    [JsonPropertyName("result")]
    public List<T> Result { get; set; } = [];

    /// <summary>
    /// 是否有更多（对于分页查询）
    /// </summary>
    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }

    //[JsonPropertyName("cached")]
    //public bool Cached { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; } = -1;

    [JsonPropertyName("id")]
    public string? Next { get; set; }

    [JsonPropertyName("extra")]
    public ArangoQueryResultExtra? Extra { get; set; }
}

public class ArangoQueryResultExtra
{
    [JsonPropertyName("stats")]
    public ArangoQueryResultStats? Stats { get; set; }
}

public class ArangoQueryResultStats
{
    [JsonPropertyName("writesExecuted")]
    public int WritesExecuted { get; set; }
}