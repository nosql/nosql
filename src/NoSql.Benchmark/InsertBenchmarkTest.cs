// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using NoSql.Benchmark;
using NoSql.Test;

public class InsertBenchmarkTest
{
    [Benchmark]
    [Arguments(DatabaseKind.Sqlite)]
    [Arguments(DatabaseKind.SqlServer)]
    [Arguments(DatabaseKind.PostgreSql)]
    public void Insert(DatabaseKind kind)
    {
        DB.GetDatabase(kind).Collection<BenchmarkPrimitiveValueObject>().Insert(BenchmarkPrimitiveValueObject.Create(1));
    }

    //[Benchmark]
    //[Arguments(DatabaseKind.Sqlite)]
    //[Arguments(DatabaseKind.SqlServer)]
    //[Arguments(DatabaseKind.PostgreSql)]
    //public void Query(DatabaseKind kind)
    //{
    //    DB.GetDatabase(kind).Table<BenchmarkPrimitiveValueObject>().Select().Where(x => x.Int > 8000).FirstOrDefault();
    //}

    //[Benchmark]
    //[Arguments(DatabaseKind.Sqlite)]
    //[Arguments(DatabaseKind.SqlServer)]
    //[Arguments(DatabaseKind.PostgreSql)]
    //public void QueryJson(DatabaseKind kind)
    //{
    //    DB.GetDatabase(kind).Table<BenchmarkJsonValueObject>().Select().Where(x => x.Object.Int > 8000).FirstOrDefault();
    //}

    //[Benchmark]
    //[Arguments(DatabaseKind.Sqlite)]
    //[Arguments(DatabaseKind.SqlServer)]
    //[Arguments(DatabaseKind.PostgreSql)]
    //public void Update(DatabaseKind kind)
    //{
    //    DB.GetDatabase(kind).Table<PrimitiveValueObject>().Update(x => x.SetProperty(p => p.Int, 0)).Where(x => x.Byte > 0).Execute();
    //}

    //[Benchmark]
    //[Arguments(DatabaseKind.Sqlite)]
    //[Arguments(DatabaseKind.SqlServer)]
    //[Arguments(DatabaseKind.PostgreSql)]
    //public void Delete(DatabaseKind kind)
    //{
    //    DB.GetDatabase(kind).Table<PrimitiveValueObject>().Delete().Where(x => x.Byte > 0).Execute();
    //}


}
