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
    /// <param name="configuration">Redis connection settings.</param>
    public RedisServiceBus(RedisStorageConfiguration configuration)
    {
        _redisStorage = new RedisStorage(configuration);
    }

    /// <inheritdoc/>
    public async Task PublishEventAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        await _redisStorage.PublishEventAsync(message, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SubscribeToEventAsync<TMessage>(Action<TMessage> handler, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        await _redisStorage.SubscribeToEventAsync(handler, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SubscribeToEventAsync<TMessage>(Func<TMessage, CancellationToken, Task> handler, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        await _redisStorage.SubscribeToEventAsync(handler, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UnsubscribeFromEventAsync<TMessage>(CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        await _redisStorage.UnsubscribeFromEventAsync<TMessage>(cancellationToken: cancellationToken);
    }
}
