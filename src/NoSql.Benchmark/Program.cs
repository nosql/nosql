// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using NoSql.Benchmark;

//DB.Sqlite.Table<BenchmarkPrimitiveValueObject>().Insert(BenchmarkPrimitiveValueObject.Create(1)).Execute();

DB.InitData();
//DB.Sqlite.Table<BenchmarkPrimitiveValueObject>().EnsureCreated();
BenchmarkRunner.Run<InsertBenchmarkTest>();
BenchmarkRunner.Run<JsonObjectVsColumnBenchmarkTest>();

Console.ReadLine();
