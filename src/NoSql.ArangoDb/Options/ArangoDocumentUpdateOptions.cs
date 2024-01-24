namespace NoSql.ArangoDb;

public class ArangoDocumentUpdateOptions
{
    public bool Silent { get; set; }
    public bool MergeObjects { get; set; }

    internal IDictionary<string, string> GetParameters()
    {
        Dictionary<string, string> parameters = new();

        if (Silent)
            parameters.Add("silent", "true");

        if (MergeObjects)
            parameters.Add("mergeObjects", "true");

        return parameters;
    }
}
