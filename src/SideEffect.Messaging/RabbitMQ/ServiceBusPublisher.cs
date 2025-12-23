using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace SideEffect.Messaging.RabbitMQ;

/// <summary>
/// Implementation of <see cref="IServiceBusPublisher"/> for RabbitMQ.
/// </summary>
/// <param name="settings">See <see cref="ServiceBusSettings"/> for more information.</param>
/// <param name="logger">See <see cref="ILogger"/> for more information.</param>
public class ServiceBusPublisher(ServiceBusSettings settings, ILogger<ServiceBusPublisher> logger)
    : ServiceBusClientBase(settings, logger: logger), IServiceBusPublisher
{
    /// <inheritdoc/>
    public async Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : EventMessage
    {
        var connection = await GetConnectionAsync(cancellationToken);

        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        var channelName = await NamingStrategy.GetChannelNameAsync<TEvent>(cancellationToken);
        var messageBody = await Serializer.SerializeObjectToBytesAsync(message, cancellationToken);

        await channel.QueueDeclareAsync(queue: channelName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: channelName, body: messageBody, cancellationToken: cancellationToken);

        Logger?.LogInformation("Event of type '{eventName}' has been published.", typeof(TEvent).Name);
    }
}
