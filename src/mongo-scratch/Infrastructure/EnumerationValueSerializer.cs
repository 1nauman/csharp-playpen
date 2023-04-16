using System.Reflection;
using MongoDB.Bson.Serialization;

namespace mongo_scratch.Infrastructure;

public class EnumerationValueSerializer<T> : IBsonSerializer<T> where T : class, IEnumerationValue
{
    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
    {
        if (value != null)
            context.Writer.WriteString(value.Value);
        else
            context.Writer.WriteNull();
    }

    public T? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var constructor = GetConstructor();
        var value = BsonSerializer.Deserialize(context.Reader, constructor.GetParameters()[0].ParameterType);

        return value == null ? default : constructor.Invoke(new[] { value }) as T;
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        if (value != null)
        {
            var iEnumValue = (IEnumerationValue)value;
            context.Writer.WriteString(iEnumValue.Value);
        }
        else
        {
            context.Writer.WriteNull();
        }
    }

    public Type ValueType => typeof(T);

    private ConstructorInfo GetConstructor()
    {
        var constructors = typeof(T).GetConstructors();
        foreach (var constructorInfo in constructors)
            if (constructorInfo.GetParameters().Length == 1)
                return constructorInfo;
        throw new NotSupportedException($"No constructor found which " +
                                        $"takes single argument for type {typeof(T).FullName}");
    }
}