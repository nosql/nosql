using NoSql.Test;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.Benchmark;

public class BenchmarkJsonValueObject : JsonValueQueryTestObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public static BenchmarkJsonValueObject Create(int seed)
    {
        const int len = 10;
        int[] arrayInt = new int[len];
        string[] arrayString = new string[len];
        PrimitiveValueQueryTestObject[] arrayObject = new PrimitiveValueQueryTestObject[len];

        for (int i = 0; i < len; i++)
        {
            arrayInt[i] = i;
            arrayString[i] = i.ToString();
            arrayObject[i] = BenchmarkPrimitiveValueObject.Create(seed);
        }
        return new BenchmarkJsonValueObject
        {
            Object = BenchmarkPrimitiveValueObject.Create(seed),
            ArrayInt = arrayInt,
            ArrayString = arrayString,
            ArrayObject = arrayObject,
            NestedObject = new JsonNestedObject
            {
                ArrayInt = arrayInt,
                Object = BenchmarkPrimitiveValueObject.Create(seed),
            }
        };
    }
}
