using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ;

/// <summary>
/// Base client of messaging hub.
/// </summary>
public class MessagingClientBase
{
    /// <summary>
    /// Gets messaging hub settings.
    /// </summary>
    protected MessagingSettings Settings { get; }

    /// <summary>
    /// Gets message serializer.
    /// </summary>
    protected IObjectSerializer Serializer { get; }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets or sets session.
    /// </summary>
    protected IConnection Connection { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="MessagingClientBase"/>.
    /// </summary>
    /// <param name="settings">See <see cref="MessagingSettings"/> for more information.</param>
    /// <param name="serializer">See <see cref="IObjectSerializer"/> for more information.</param>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected MessagingClientBase(MessagingSettings settings, IObjectSerializer serializer, ILogger logger)
    {
        Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Establishes connection with messaging hub.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    protected async Task EstablishConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (Connection is not null)
        {
            return;
        }

        var connectionFactory = new ConnectionFactory { Uri = new Uri(Settings.ConnectionString) };
        Connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
    }

    /// <summary>
    /// Closes connection with messaging hub.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    protected async Task CloseConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (Connection is null)
        {
            return;
        }

        await Connection.CloseAsync(cancellationToken);
        Connection = null;
    }
}
