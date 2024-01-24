namespace NoSql.ArangoDb;

public class ArangoDocumentCreateOptions
{
    internal static ArangoDocumentCreateOptions Default = new();
    public bool Silent { get; set; } = true;
    public bool Overwrite { get; set; }
    public bool MergeObjects { get; set; }
    public ArangoOverwriteMode OverwriteMode { get; set; } = ArangoOverwriteMode.Conflict;

    internal IDictionary<string, string> GetParameters()
    {
        Dictionary<string, string> parameters = new();

        if (Silent)
            parameters.Add("silent", "true");

        if (Overwrite)
            parameters.Add("overwrite", "true");

        if (MergeObjects)
            parameters.Add("mergeObjects", "true");

        if (OverwriteMode != ArangoOverwriteMode.Conflict)
            parameters.Add("overwriteMode", OverwriteMode.ToString().ToLower());

        return parameters;
    }
}
