using Microsoft.Extensions.Logging;
using SideEffect.Messaging.Naming;
using SideEffect.Messaging.Serialization;
using StackExchange.Redis;

namespace SideEffect.Messaging.Redis;

/// <summary>
/// Base logic for interaction with Redis service bus provider.
/// </summary>
public abstract class ServiceBusClientBase
{
    private readonly ServiceBusSettings _settings;

    private IConnectionMultiplexer _connection;

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
    /// <returns>See <see cref="IConnectionMultiplexer"/> for more information.</returns>
    protected async Task<IConnectionMultiplexer> GetConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (_connection is null)
        {
            var options = ConfigureConnection();
            _connection = await ConnectionMultiplexer.ConnectAsync(options);
        }

        return _connection;
    }

    /// <summary>
    /// Creates Redis channel.
    /// </summary>
    /// <typeparam name="TEvent">Type of event message.</typeparam>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="RedisChannel"/> for more information.</returns>
    protected async Task<RedisChannel> GetChannelAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : EventMessage
    {
        var channelName = await NamingStrategy.GetChannelNameAsync<TEvent>(cancellationToken);
        return new RedisChannel(channelName, RedisChannel.PatternMode.Literal);
    }

    /// <summary>
    /// Creates Redis value to publish.
    /// </summary>
    /// <typeparam name="TEvent">Type of event message.</typeparam>
    /// <param name="message">See <see cref="EventMessage"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="RedisValue"/> for more information.</returns>
    protected async Task<RedisValue> GetValueAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
    {
        var messageBody = await Serializer.SerializeObjectToStringAsync(message, cancellationToken);
        return new RedisValue(messageBody);
    }

    private ConfigurationOptions ConfigureConnection()
    {
        var options = ConfigurationOptions.Parse(_settings.ConnectionString);
        options.Password = _settings.Password;

        return options;
    }
}
