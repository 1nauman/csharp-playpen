using Newtonsoft.Json;

namespace niit_api_client;

/// <summary>
/// <see cref="IJsonSerializer"/> implementation using Json.NET.
/// </summary>
public class DefaultJsonSerializer : IJsonSerializer
{
    /// <inheritdoc />
    public string Serialize(object value) => JsonConvert.SerializeObject(value);

    /// <inheritdoc />
    public T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value);
}