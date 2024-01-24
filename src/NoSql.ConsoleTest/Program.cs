using Microsoft.Extensions.DependencyInjection;
using NoSql;
using NoSql.ConsoleTest;
using System.ComponentModel.DataAnnotations.Schema;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //ISqlDatabase db = CreateSqliteDatabase();
        ISqlDatabase db = CreatePostgreSqlDatabase();
        //ISqlDatabase db = CreateSqlServerDatabase();

        //await db.Table<FakeObject>().DropAsync();
        if (await db.Table<FakeObject>().EnsureCreatedAsync() == NoSql.Scaffolding.TableCreateResult.Created)
        {
            for (int i = 0; i < 100; i++)
            {
                await db.Table<FakeObject>().Insert(FakeObject.Create()).ExecuteAsync();
            }
        }

        var query = db.Table<FakeObject>()
            .Select(x => new
            {
                x.Object.String,
                x.Object!.Bool,
                x.Object!.Int32,
                X = x.Object.Struct,
            })
            .Where(x => x.Byte > 100 && x.Struct.Integer > 0)
            .OrderBy(x => x.Struct.Integer);

        var sql = query.ToQueryString();
        var data = await query.ToListAsync();

        var updateQuery = db.Table<FakeObject>()
            .Update(setter => setter.SetProperty(x => x.Object.String, "aaa"))
            .Where(x => x.Object != null);

        var updateSql = updateQuery.ToQueryString();
        await updateQuery.ExecuteAsync();

        Console.ReadLine();

        //await NewMethod(db);


        //var sql2 = db.Table<Phone>().Update(x => new Phone
        //{
        //    TaskId = 0,
        //    Data = new PhoneData
        //    {
        //        Title = "3",
        //    }
        //}).Where(x => x.TaskId == 1).ToQueryString();

        //var sql2 = db.Table<Phone>().Update(x => new Phone
        //{
        //    TaskId = 0,
        //    Data = new PhoneData
        //    {
        //        Title = "3",
        //    }
        //}).Where(x => x.TaskId == 1).ToQueryString();


    }

    private static async Task NewMethod(ISqlDatabase db)
    {
        var phones2 = await db.Table<Phone>()
            .Select(x => x.TaskId)
            .Where(x => x.TaskId == 8398824968192)
            .OrderBy(x => x.Data.CreateTime, false)
            .Limit(10)
            .ToListAsync();

        var phones = await db.Table<Phone>()
            .Select(x => new
            {
                A = x.Data,
                B = new
                {
                    x.Data.Summary,
                    x.Data.Link,
                },
                x.TaskId,
                x.Id
            })
            .Where(x => x.TaskId == 8398824968192)
            .OrderBy(x => x.Data.CreateTime, false)
            .Limit(10)
            .ToListAsync();

        var sql = db.Table<Phone>()
            .Delete()
            .Where(x => x.Data.Summary == "aa" && x.TaskId == 8398824968192)
            .ToQueryString();

        var sql1 = db.Table<Phone>()
            .Where(x => x.TaskId == 1)
            .Update(setter => setter
                .SetProperty(x => x.Data.Summary, "")
                .SetProperty(x => x.Id, x => x.Id))
            .ToQueryString();

        var sql2 = db.Table<Phone>().Insert(new Phone
        {
            Id = 1,
            Data = new PhoneData
            {
                Keyword = "a",
            },
            TaskId = 1,
            Robot = "robot",
            CreateTime = -1,
        }).ToQueryString();

        Console.WriteLine(sql);

    }

    private static ISqlDatabase CreateSqliteDatabase()
    {
        return new ServiceCollection()
            .AddNoSql(builder=>builder.UseSqlite("Data Source=C:\\Dev\\temp\\SqliteNoSql\\SqliteNoSql\\bin\\Debug\\net7.0\\khaos.db"))
            .BuildServiceProvider()
            .GetRequiredService<ISqlDatabase>();
    }

    private static ISqlDatabase CreateSqlServerDatabase()
    {
        return new ServiceCollection()
            .AddNoSql(builder => builder.UseSqlServer("Data Source=.;Database=JsonDb;uid=sa;pwd=P@ssw0rd;TrustServerCertificate=true"))
            .BuildServiceProvider()
            .GetRequiredService<ISqlDatabase>();
    }

    private static ISqlDatabase CreatePostgreSqlDatabase()
    {
        return new ServiceCollection()
            .AddNoSql(builder => builder.UsePostgreSql("Host=localhost;Port=5433;Database=JsonDb;User Id=postgres;Password=P@ssw0rd"))
            .BuildServiceProvider()
            .GetRequiredService<ISqlDatabase>();
    }

}

[Table("Users")]
public class User
{
    public string Name { get; set; }
    public int Id { get; set; }
    public bool? Sex { get; set; }
    public byte[] Bin { get; set; }

    public Department Department { get; set; }
}

[Table("Departments")]
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }

    public User Manager { get; set; }
}

[Table("g_phone_2")]
public class Phone
{
    public int Id { get; set; }
    public PhoneData Data { get; set; }
    public long TaskId { get; set; }
    public string Robot { get; set; }
    public long CreateTime { get; set; }
}

public class PhoneData
{
    public string? Keyword { get; set; }
    public string[]? Phone { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? Link { get; set; }
    public DateTime CreateTime { get; set; }
}


public class JsonTable
{
    public Guid Id { get; set; }
    public JsonTableData Json { get; set; }
}

public class JsonTableData
{
    public int A { get; set; }
}

public class TestModel
{

}