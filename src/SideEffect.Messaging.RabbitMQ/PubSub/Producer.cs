using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ.PubSub;

/// <summary>
/// Pub/Sub event producer.
/// </summary>
public class Producer : MessagingClientBase, IProducer, IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of <see cref="Producer"/>.
    /// </summary>
    /// <param name="settings">See <see cref="MessagingSettings"/> for more information.</param>
    /// <param name="serializer">See <see cref="IObjectSerializer"/> for more information.</param>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Producer(MessagingSettings settings, IObjectSerializer serializer, Logger<Producer> logger)
        : base(settings, serializer, logger)
    { }

    /// <inheritdoc/>
    public async Task PublishEventAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : Event
    {
        await EstablishConnectionAsync(cancellationToken);

        using var channel = await Connection.CreateChannelAsync(cancellationToken: cancellationToken);

        var exchangeName = typeof(TEvent).Name;

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var messageBody = await Serializer.SerializeObjectToBytesAsync(message, cancellationToken);

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: string.Empty,
            body: messageBody,
            cancellationToken: cancellationToken);

        Logger?.LogInformation("Event of type '{messageType}' has been published.", typeof(TEvent).Name);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await CloseConnectionAsync(CancellationToken.None);
    }
}
