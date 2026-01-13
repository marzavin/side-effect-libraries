using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SideEffect.Messaging.Naming;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ;

/// <summary>
/// Base logic for interaction with RabbitMQ service bus provider.
/// </summary>
public abstract class ServiceBusClientBase
{
    private readonly ServiceBusSettings _settings;

    private IConnection _connection;

    /// <summary>
    /// Gets object serializer.
    /// </summary>
    protected IObjectSerializer Serializer { get; }

    /// <summary>
    /// Gets naming strategy for message channel.
    /// </summary>
    protected INamingStrategy NamingStrategy { get; }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ServiceBusClientBase"/>.
    /// </summary>
    /// <param name="settings">See <see cref="ServiceBusSettings"/> for more information.</param>
    /// <param name="serializer">See <see cref="IObjectSerializer"/> for more information.</param>
    /// <param name="namingStrategy">See <see cref="INamingStrategy"/> for more information.</param>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    protected ServiceBusClientBase(
        ServiceBusSettings settings,
        IObjectSerializer serializer = null,
        INamingStrategy namingStrategy = null,
        ILogger logger = null)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        Serializer = serializer ?? new JsonObjectSerializer();
        NamingStrategy = namingStrategy ?? new TypeBasedNamingStrategy();
        Logger = logger;
    }

    /// <summary>
    /// Returns (or creates new) connection with service bus provider.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="IConnection"/> for more information.</returns>
    protected async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_connection is null)
        {
            var connectionFactory = ConfigureConnection();
            _connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        }

        return _connection;
    }

    private ConnectionFactory ConfigureConnection()
    {
        return new ConnectionFactory { Uri = new Uri(_settings.ConnectionString) };
    }
}
