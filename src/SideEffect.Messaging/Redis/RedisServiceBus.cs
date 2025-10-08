namespace SideEffect.Messaging.Redis;

/// <summary>
/// Service bus implementation based on Redis channels.
/// </summary>
public class RedisServiceBus : IServiceBus
{
    private RedisStorage _redisStorage;

    /// <summary>
    /// Creates new instance of Redis service bus implementation.
    /// </summary>
    /// <param name="connectionString">Redis connection string.</param>
    public RedisServiceBus(string connectionString)
    {
        _redisStorage = new RedisStorage(connectionString);
    }

    /// <inheritdoc/>
    public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        await _redisStorage.PublishAsync(message, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SubscribeAsync<TMessage>(Action<TMessage> handler, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        await _redisStorage.SubscribeAsync(handler, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public Task UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
