namespace NoSql.ArangoDb;

public class AqlQueryOptions
{
    public static AqlQueryOptions Default = new();
    public bool Count { get; set; } = false;
    public int BatchSize { get; set; } = 1000;
}
