using System.Data.Common;
using System.Runtime.CompilerServices;

namespace NoSql.Storage;

public abstract class RelationalDbConnection : INoSqlDbConnection
{
    public abstract DbConnection DbConnection { get; }

    public T? ExecuteOneOrDefaultOne<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields)
    {
        using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();

        try
        {
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return Read<T>(reader, tableInfo, fields, default);
            }

            return default;
        }
        finally
        {
            DbConnection.Close();
        }
    }

    public async Task<T?> ExecuteOneOrDefaultAsync<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields, CancellationToken cancellationToken = default)
    {
        await using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();

        try
        {
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            if (await reader.ReadAsync(cancellationToken))
            {
                return Read<T>(reader, tableInfo, fields, cancellationToken);
            }

            return default;
        }
        finally
        {
            await DbConnection.CloseAsync();
        }
    }

    public IEnumerable<T?> ExecuteEnumerable<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields)
    {
        using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();

        try
        {
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return Read<T>(reader, tableInfo, fields, default);
            }
        }
        finally
        {
            DbConnection.Close();
        }
    }

    public async IAsyncEnumerable<T?> ExecuteEnumerableAsync<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();

        try
        {
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                yield return Read<T>(reader, tableInfo, fields, cancellationToken);
            }
        }
        finally
        {
            await DbConnection.CloseAsync();
        }
    }

    public int ExecuteNonQuery(string sql)
    {
        using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();

        try
        {
            return command.ExecuteNonQuery();
        }

        finally
        {
            DbConnection.Close();
        }
    }

    public async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken)
    {
        using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();

        try
        {
            return await command.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            await DbConnection.CloseAsync();
        }
    }

    public T? ExecuteScalar<T>(string sql)
    {
        using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();
        try
        {
            var result = command.ExecuteScalar();
            if (result == default)
                return default;
            return (T)Convert.ChangeType(result, typeof(T));
        }
        finally
        {
            DbConnection.Close();
        }
    }

    public async Task<T?> ExecuteScalarAsync<T>(string sql, CancellationToken cancellationToken)
    {
        await using var command = DbConnection.CreateCommand();
        command.CommandText = sql;
        DbConnection.Open();

        try
        {
            var result = await command.ExecuteScalarAsync(cancellationToken);
            if (result == default)
                return default;
            return (T)Convert.ChangeType(result, typeof(T));
        }
        finally
        {
            await DbConnection.CloseAsync();
        }
    }

    private static T? Read<T>(DbDataReader reader, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields, CancellationToken cancellationToken)
    {
        // ValueType
        if (fields == null)
        {
            var value = tableInfo.TypeMapping.ReadFromDataReader(reader, 0);
            return value == null ? default : (T?)value;
        }

        // AnonymousType
        if (tableInfo.RequireConstructorParameters)
        {
            var args = new object?[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                args[i] = fields[i].TypeMapping.ReadFromDataReader(reader, i);
            }
            return (T)tableInfo.CreateInstance!(args);
        }
        else
        {
            var instance = (T)tableInfo.CreateInstance!(null);
            for (int i = 0; i < fields.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var column = fields[i];
                column.Set?.Invoke(instance, column.TypeMapping.ReadFromDataReader(reader, i));
            }
            return instance;
        }
    }

}