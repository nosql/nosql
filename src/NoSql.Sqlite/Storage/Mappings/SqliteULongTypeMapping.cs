using NoSql.Storage.Mappings;

namespace NoSql.Sqlite.Storage.Mappings;

public class SqliteULongTypeMapping : ULongTypeMapping
{
    public SqliteULongTypeMapping(string storeType) : base(storeType) { }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    protected override string GenerateNonNullSqlLiteral(object value)
        => new LongTypeMapping(StoreType).GenerateSqlLiteral((long)(ulong)value);
}
