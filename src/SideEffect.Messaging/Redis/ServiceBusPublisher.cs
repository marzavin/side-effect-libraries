using Microsoft.Extensions.Logging;

namespace SideEffect.Messaging.Redis;

/// <summary>
/// Implementation of <see cref="IServiceBusPublisher"/> for Redis.
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
        var channel = await GetChannelAsync<TEvent>(cancellationToken);
        var value = await GetValueAsync(message, cancellationToken);
        
        var connection = await GetConnectionAsync(cancellationToken);

        var subscriber = connection.GetSubscriber();
        await subscriber.PublishAsync(channel, value);

        Logger?.LogInformation("Event of type '{eventName}' has been published.", typeof(TEvent).Name);
    }
}
