using System.Text.Json.Serialization;

namespace NoSql.ArangoDb;

/// <summary>
/// Arango执行结果基类
/// </summary>
internal class ArangoResult
{
    /// <summary>
    /// 结果代码，大多数情况是Http StatusCode
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// 是否异常
    /// </summary>
    [JsonPropertyName("error")]
    public bool Error { get; set; }

    /// <summary>
    /// 异常代码
    /// </summary>
    [JsonPropertyName("errorNum")]
    public int ErrorNum { get; set; }

    /// <summary>
    /// 异常消息
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}
