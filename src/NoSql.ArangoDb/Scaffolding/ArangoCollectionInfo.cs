using System.Text.Json.Serialization;

namespace NoSql.ArangoDb.Scaffolding;

/// <summary>
/// Arango Collection信息
/// </summary>
public class ArangoCollectionInfo
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 是否是系统Collection（内建）
    /// </summary>
    [JsonPropertyName("isSystem")]
    public bool IsSystem { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 唯一ID
    /// </summary>
    [JsonPropertyName("globallyUniqueId")]
    public string GloballyUniqueId { get; set; } = null!;

    /// <summary>
    /// 类型
    /// </summary>
    [JsonPropertyName("type")]
    public ArangoCollectionType Type { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [JsonPropertyName("status")]
    public ArangoCollectionStatus Status { get; set; }
}
