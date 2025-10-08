namespace SideEffect.Messaging.Redis;

/// <summary>
/// Configuration of Redis storage.
/// </summary>
public class RedisStorageConfiguration
{
    /// <summary>
    /// Gets or sets connection string.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }
}
