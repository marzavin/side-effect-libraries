namespace SideEffect.Messaging.RabbitMQ;

/// <summary>
/// Configuration of RabbitMQ service bus.
/// </summary>
public class ServiceBusSettings
{
    /// <summary>
    /// Gets or sets user name.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// Gets or sets user password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets virtual host.
    /// </summary>
    public string VirtualHost { get; set; }

    /// <summary>
    /// Gets or sets host.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets port.
    /// </summary>
    public int? Port { get; set; }
}
