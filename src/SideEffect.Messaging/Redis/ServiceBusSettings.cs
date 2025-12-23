namespace SideEffect.Messaging.Redis;

/// <summary>
/// Configuration of Redis service bus.
/// </summary>
public class ServiceBusSettings
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
