using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace newtonsoft_playpen;

public class EmployeeConverter : JsonConverter<Employee>
{
    // Do not use this converter for reading, even if it is supplied to deserialize method, it'll be ignored
    public override bool CanRead => false;

    public override void WriteJson(JsonWriter writer, Employee? value, JsonSerializer serializer)
    {
        var t = JToken.FromObject(value);

        if (t.Type != JTokenType.Object)
        {
            t.WriteTo(writer);
        }
        else
        {
            var o = (JObject)t;
            IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

            o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

            o.WriteTo(writer);
        }
    }

    public override Employee? ReadJson(JsonReader reader, Type objectType, Employee? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer) => throw new NotImplementedException();
}