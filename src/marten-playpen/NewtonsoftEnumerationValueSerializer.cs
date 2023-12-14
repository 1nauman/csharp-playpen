using System.Reflection;
using Newtonsoft.Json;

namespace marten_playpen;

public class NewtonsoftEnumerationValueSerializer : JsonConverter
{
    private static readonly Type[] FromValueParamTypes = { typeof(string) };
        
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue((value as IEnumValue)?.Value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        if (!(reader.Value is string value))
        {
            return null;
        }

        var fromValueMethod = objectType.GetMethod(
            "FromValue", BindingFlags.Static | BindingFlags.Public, null,
            FromValueParamTypes, null
        );
        if (fromValueMethod == null)
        {
            throw new NotSupportedException($"Type {objectType.FullName} does not implement " +
                                            $"public static FromValue(string) method. ");
        }

        return fromValueMethod.Invoke(null, new [] {value});
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.GetInterfaces().Any(x => x == typeof(IEnumValue));
    }
}