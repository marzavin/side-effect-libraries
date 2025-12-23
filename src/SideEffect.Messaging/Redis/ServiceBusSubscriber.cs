using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SideEffect.Messaging.Handlers;

namespace SideEffect.Messaging.Redis;

/// <summary>
/// Implementation of <see cref="IServiceBusSubscriber"/> for Redis.
/// </summary>
public class ServiceBusSubscriber : ServiceBusClientBase, IServiceBusSubscriber
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="ServiceBusSubscriber"/>.
    /// </summary>
    /// <param name="settings">See <see cref="ServiceBusSettings"/> for more information.</param>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <param name="serviceProvider">See <see cref="IServiceProvider"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ServiceBusSubscriber(
        ServiceBusSettings settings,
        ILogger<ServiceBusSubscriber> logger,
        IServiceProvider serviceProvider)
        : base(settings, logger: logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc/>
    public async Task SubscribeToEventAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : EventMessage
    {
        var channel = await GetChannelAsync<TEvent>(cancellationToken);

        var connection = await GetConnectionAsync(cancellationToken);

        var subscriber = connection.GetSubscriber();
        await subscriber.SubscribeAsync(channel, async (channel, value) =>
        {
            using var scope = _serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<EventHandlerBase<TEvent>>()?.ToList();

            if (handlers is not null && handlers.Count > 0)
            {
                var message = await Serializer.DeserializeObjectFromStringAsync<TEvent>(value.ToString());
                var tasks = handlers.Select(x => x.HandleAsync(message, cancellationToken));
                await Task.WhenAll(tasks);
            }
        });
    }

    /// <inheritdoc/>
    public async Task UnsubscribeAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : EventMessage
    {
        var channel = await GetChannelAsync<TEvent>(cancellationToken);

        var connection = await GetConnectionAsync(cancellationToken);

        var subscriber = connection.GetSubscriber();
        await subscriber.UnsubscribeAsync(channel);
    }
}
