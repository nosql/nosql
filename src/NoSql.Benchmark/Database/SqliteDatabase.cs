// See https://aka.ms/new-console-template for more information
using NoSql;

public class SqliteDatabase : NoSqlDatabase
{
    public SqliteDatabase(SqlDatabaseDependencies<SqliteDatabase> options) : base(options) { }
}

public class SqliteWithJsonObjectDatabase : NoSqlDatabase
{
    public SqliteWithJsonObjectDatabase(SqlDatabaseDependencies<SqliteWithJsonObjectDatabase> options) : base(options) { }
}