// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using NoSql;

public class SqlServerDatabase : NoSqlDatabase
{
    public SqlServerDatabase(SqlDatabaseDependencies<SqlServerDatabase> options) : base(options) { }
}
