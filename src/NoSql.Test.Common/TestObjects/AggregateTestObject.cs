using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.Test;

public class AggregateTestObject
{
    public AggregateTestObject() { }

    public AggregateTestObject(int intValue,double doubleValue)
    {
        Int = intValue;
        Double = doubleValue;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Int { get; set; }
    public double Double { get; set; }
    public int FixedValue { get; set; } = 1;
}