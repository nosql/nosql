namespace NoSql.ArangoDb.Scaffolding;

/// <summary>
/// 集合状态
/// </summary>
public enum ArangoCollectionStatus
{
    NewBorn = 1,
    Unloaded,
    Loaded,
    BeingUnloaded,
    Deleted,
    Loading,
}