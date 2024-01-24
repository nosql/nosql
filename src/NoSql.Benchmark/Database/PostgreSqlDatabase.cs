// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using NoSql;

public class PostgreSqlDatabase : NoSqlDatabase
{
    public PostgreSqlDatabase(SqlDatabaseDependencies<PostgreSqlDatabase> options) : base(options) { }
}