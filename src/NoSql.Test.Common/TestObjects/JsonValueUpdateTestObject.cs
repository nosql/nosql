namespace NoSql.Test;

public class JsonValueUpdateTestObject : JsonValueQueryTestObject
{
    public PrimitiveValueQueryTestObject UpdateObject { get; set; }

    public static new JsonValueUpdateTestObject Create()
    {
        const int len = 10;
        int[] arrayInt = new int[len];
        string[] arrayString = new string[len];
        PrimitiveValueQueryTestObject[] arrayObject = new PrimitiveValueQueryTestObject[len];

        for (int i = 0; i < len; i++)
        {
            arrayInt[i] = i;
            arrayString[i] = i.ToString();
            arrayObject[i] = PrimitiveValueQueryTestObject.Create();
        }
        return new JsonValueUpdateTestObject
        {
            Object = PrimitiveValueQueryTestObject.Create(),
            UpdateObject = PrimitiveValueQueryTestObject.Create(),
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
