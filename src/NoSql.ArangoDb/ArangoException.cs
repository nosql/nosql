namespace NoSql.ArangoDb;

/// <summary>
/// Arango调用异常
/// </summary>
public class ArangoException : Exception
{
    public ArangoException() { }

    public ArangoException(int code)
    {
        Code = code;
    }

    internal ArangoException(ArangoResult? error) : base(error?.ErrorMessage)
    {
        if (error != null)
        {
            Code = error.Code;
            ErrorNum = error.ErrorNum;
        }
    }

    /// <summary>
    /// 代码，大多数时候为Http StatusCode
    /// </summary>
    public int Code { get; }

    /// <summary>
    /// 异常代码
    /// </summary>
    public int ErrorNum { get; }
}
