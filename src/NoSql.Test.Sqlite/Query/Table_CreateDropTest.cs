using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.Test;

[TestClass]
public class Table_CreateDropTest
{
    [TestMethod]
    public void CreateDrop_WithName()
    {
        var table = DB.Table<TempTable1>("temp1");
        table.EnsureCreated();
        table.Drop();
    }

    [TestMethod]
    public void Migrate()
    {
        DB.Table<TempTable1>("temp1").EnsureCreated();
        DB.Table<TempTable2>("temp1").EnsureCreated();
        DB.Table<TempTable2>("temp1").Drop();
    }

    public class TempTable1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }

    public class TempTable2 : TempTable1
    {
        public int Field { get; set; }
    }

}
