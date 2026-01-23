using Newtonsoft.Json;
using System.Text;

namespace SideEffect.Messaging.Serialization;

/// <summary>
/// Implementation of <see cref="IObjectSerializer"/> that uses JSON as serialization format.
/// </summary>
public class JsonObjectSerializer : IObjectSerializer
{
    private readonly JsonSerializerSettings _options = new() { NullValueHandling = NullValueHandling.Ignore };

    /// <inheritdoc/>
    public async Task<TObject> DeserializeObjectFromBytesAsync<TObject>(byte[] data, CancellationToken cancellationToken = default)
    {
        var jsonString = Encoding.UTF8.GetString(data);
        return await DeserializeObjectFromStringAsync<TObject>(jsonString, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<TObject> DeserializeObjectFromStringAsync<TObject>(string data, CancellationToken cancellationToken = default)
    {
        var objectFromJson = JsonConvert.DeserializeObject<TObject>(data, _options);
        return Task.FromResult(objectFromJson);
    }

    /// <inheritdoc/>
    public async Task<byte[]> SerializeObjectToBytesAsync<TObject>(TObject data, CancellationToken cancellationToken = default)
    {
        var jsonString = await SerializeObjectToStringAsync(data, cancellationToken);
        return Encoding.UTF8.GetBytes(jsonString);
    }

    /// <inheritdoc/>
    public Task<string> SerializeObjectToStringAsync<TObject>(TObject data, CancellationToken cancellationToken = default)
    {
        var jsonString = JsonConvert.SerializeObject(data, _options);
        return Task.FromResult(jsonString);
    }
}
