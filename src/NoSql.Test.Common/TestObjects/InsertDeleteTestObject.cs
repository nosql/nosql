using System.ComponentModel.DataAnnotations;

namespace NoSql.Test;

public class InsertDeleteTestObject
{
    [Key]
    public int Id { get; set; }

    public InsertDeleteTestObject() { }

    public InsertDeleteTestObject(int id)
    {
        Id = id;
    }
}
