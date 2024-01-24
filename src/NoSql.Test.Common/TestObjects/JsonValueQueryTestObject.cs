namespace NoSql.Test;
public class JsonValueQueryTestObject
{
    public PrimitiveValueQueryTestObject Object { get; set; }
    public PrimitiveValueQueryTestObject? NullObject { get; set; }

    public JsonNestedObject NestedObject { get; set; }
    public int[] ArrayInt { get; set; }
    public string[] ArrayString { get; set; }

    public PrimitiveValueQueryTestObject[] ArrayObject { get; set; }

    public Dictionary<string, object> Directory { get; set; }

    public static JsonValueQueryTestObject Create()
    {
        const int len = 10;
        int[] arrayInt = new int[len];
        string[] arrayString = new string[len];
        PrimitiveValueQueryTestObject[] arrayObject = new PrimitiveValueQueryTestObject[len];

        for (int i = 0; i < len; i++)
        {
            arrayInt[i] = i;
            arrayString[i] = i.ToString();
            arrayObject[i] = PrimitiveValueQueryTestObject.Create(i);
        }
        return new JsonValueQueryTestObject
        {
            Object = PrimitiveValueQueryTestObject.Create(),
            ArrayInt = arrayInt,
            ArrayString = arrayString,
            ArrayObject = arrayObject,
            NestedObject = new JsonNestedObject
            {
                ArrayInt = arrayInt,
                Object = PrimitiveValueQueryTestObject.Create(),
            }
        };
    }
}

public class JsonNestedObject
{
    public int[] ArrayInt { get; set; }
    public PrimitiveValueQueryTestObject Object { get; set; }
}
