using StackExchange.Redis;
using System.Text.Json;

namespace SideEffect.Messaging.Redis;

internal class RedisStorage
{
    private readonly ConnectionMultiplexer _connection;

    public RedisStorage(RedisStorageConfiguration configuration)
    {
        var options = ConfigurationOptions.Parse(configuration.ConnectionString);
        options.Password = configuration.Password;

        _connection = ConnectionMultiplexer.Connect(options);
    }

    public async Task PublishEventAsync<TMessage>(TMessage message, string channelName = null, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        var channel = GetChannel<TMessage>(channelName);
        var value = new RedisValue(JsonSerializer.Serialize(message));

        var subscriber = _connection.GetSubscriber();
        await subscriber.PublishAsync(channel, value);
    }

    public async Task SubscribeToEventAsync<TMessage>(Func<TMessage, CancellationToken, Task> handler, string channelName = null, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        var channel = GetChannel<TMessage>(channelName);

        var subscriber = _connection.GetSubscriber();
        await subscriber.SubscribeAsync(channel, (channel, value) => Task.Run(() =>
        {
            var message = JsonSerializer.Deserialize<TMessage>(value);
            return handler.Invoke(message, cancellationToken);
        }));
    }

    public async Task SubscribeToEventAsync<TMessage>(Action<TMessage> handler, string channelName = null, CancellationToken cancellationToken = default)
        where TMessage : IMessage
    {
        var channel = GetChannel<TMessage>(channelName);

        var subscriber = _connection.GetSubscriber();
        await subscriber.SubscribeAsync(channel, (channel, value) =>
        {
            var message = JsonSerializer.Deserialize<TMessage>(value);
            handler.Invoke(message);
        });
    }

    private static RedisChannel GetChannel<TMessage>(string channelName)
        where TMessage : IMessage
    {
        var name = string.IsNullOrWhiteSpace(channelName) ? typeof(TMessage).FullName : channelName;
        return new RedisChannel(name, RedisChannel.PatternMode.Literal);
    }
}
