using NoSql.ArangoDb.Storage;

namespace NoSql.ArangoDb;

public class ArangoDatabase(NoSqlOptions options, NoSqlDependencies dependencies) : NoSqlDatabase(dependencies)
{
    public override NoSqlCollection<T> Collection<T>(string? tableName = null) => new ArangoCollection<T>(options, Dependencies, tableName);

    private class ArangoCollection<T>(
        NoSqlOptions options,
        NoSqlDependencies dependency,
        string? name) : NoSqlCollection<T>(dependency, name)
    {
        public override T? Find(params object[] key) => FindAsync(key, CancellationToken.None).Result;
        public override int Insert(T value) => InsertAsync(value).Result;
        public override int Insert(params T[] values) => InsertAsync(values).Result;

        public override Task<T?> FindAsync(object key, CancellationToken cancellationToken = default)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var connection = (ArangoDbConnection)Dependencies.Connection;
            return connection.CreateHttpClient().GetDocumentAsync<T>(TypeInfo.Name, key.ToString()!, options.SerializerOptions, cancellationToken);
        }

        public override Task<T?> FindAsync(object[] key, CancellationToken cancellationToken = default)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key.Length != 1)
                throw new NotSupportedException("Composite primary key not supported.");

            return FindAsync(key[0], cancellationToken);
        }

        public override async Task<int> InsertAsync(T value, CancellationToken cancellationToken = default)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var connection = (ArangoDbConnection)Dependencies.Connection;
            await connection.CreateHttpClient().PostDocumentAsync(TypeInfo.Name, value, new ArangoDocumentCreateOptions
            {
                MergeObjects = false,
                Overwrite = false,
                Silent = false,
            }, options.SerializerOptions, cancellationToken);
            return 1;
        }

        public override async Task<int> InsertAsync(T[] values, CancellationToken cancellationToken = default)
        {
            var connection = (ArangoDbConnection)Dependencies.Connection;
            var results = await connection.CreateHttpClient().PostDocumentAsync(TypeInfo.Name, values, new ArangoDocumentCreateOptions
            {
                MergeObjects = false,
                Overwrite = false,
                Silent = false,
            }, options.SerializerOptions, cancellationToken);
            return results.Length;
        }

    }
}

