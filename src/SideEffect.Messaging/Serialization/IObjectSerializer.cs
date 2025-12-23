namespace SideEffect.Messaging.Serialization;

/// <summary>
/// Base interface for object serializer.
/// </summary>
public interface IObjectSerializer
{
    /// <summary>
    /// Serializes object to byte array.
    /// </summary>
    /// <typeparam name="TObject">Type of object.</typeparam>
    /// <param name="data">Content to serialize.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Returns serialized data (byte array).</returns>
    Task<byte[]> SerializeObjectToBytesAsync<TObject>(TObject data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Serializes object to string.
    /// </summary>
    /// <typeparam name="TObject">Type of object.</typeparam>
    /// <param name="data">Content to serialize.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Returns serialized data (string).</returns>
    Task<string> SerializeObjectToStringAsync<TObject>(TObject data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deserializes object from byte array.
    /// </summary>
    /// <typeparam name="TObject">Type of object.</typeparam>
    /// <param name="data">Content to deserialize.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Returns deserialized data.</returns>
    Task<TObject> DeserializeObjectFromBytesAsync<TObject>(byte[] data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deserializes object from string.
    /// </summary>
    /// <typeparam name="TObject">Type of object.</typeparam>
    /// <param name="data">Content to deserialize.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Returns deserialized data.</returns>
    Task<TObject> DeserializeObjectFromStringAsync<TObject>(string data, CancellationToken cancellationToken = default);
}
