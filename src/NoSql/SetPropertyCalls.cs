namespace NoSql;

public sealed class SetPropertyCalls<T>
{
    public SetPropertyCalls<T> SetProperty<TProperty>(Func<T, TProperty?> property, Func<T, TProperty?> value) => throw new NotImplementedException();
    public SetPropertyCalls<T> SetProperty<TProperty>(Func<T, TProperty?> property, TProperty? value) => throw new NotImplementedException();
}
