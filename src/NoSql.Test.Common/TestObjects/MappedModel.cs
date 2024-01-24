using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NoSql.Test;

public class MappedModel
{
    [Column("i")]
    [JsonPropertyName("i")]
    [Key]
    public int Id { get; set; }

    [Column("n")]
    [JsonPropertyName("n")]
    public string Name { get; set; }
}