using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.Handlers;

namespace SideEffect.Messaging.RabbitMQ;

/// <summary>
/// Implementation of <see cref="IServiceBusSubscriber"/> for RabbitMQ.
/// </summary>
public class ServiceBusSubscriber : ServiceBusClientBase, IServiceBusSubscriber
{
    private AsyncEventingBasicConsumer _consumer;

    private IChannel _channel;

    private AsyncEventHandler<BasicDeliverEventArgs> _messageHandler;

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
        if (_consumer is not null)
        {
            await UnsubscribeFromEventAsync<TEvent>(cancellationToken);
        }

        var connection = await GetConnectionAsync(cancellationToken);

        _channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        var channelName = await NamingStrategy.GetChannelNameAsync<TEvent>(cancellationToken);

        await _channel.QueueDeclareAsync(queue: channelName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

        _messageHandler = async (object sender, BasicDeliverEventArgs ea) =>
        {
            Logger?.LogInformation("Event of type '{eventName}' has been received.", typeof(TEvent).Name);

            using var scope = _serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<EventHandlerBase<TEvent>>()?.ToList();

            if (handlers is not null && handlers.Count > 0)
            {
                var message = await Serializer.DeserializeObjectFromBytesAsync<TEvent>(ea.Body.ToArray());
                var tasks = handlers.Select(x => x.HandleAsync(message, cancellationToken));
                await Task.WhenAll(tasks);
            }

            Logger?.LogInformation("Event of type '{eventName}' has been handled.", typeof(TEvent).Name);
        };

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.ReceivedAsync += _messageHandler;

        await _channel.BasicConsumeAsync(channelName, true, _consumer, cancellationToken: cancellationToken);

        Logger?.LogInformation("Subscription to '{eventName}' event has been added.", typeof(TEvent).Name);
    }

    /// <inheritdoc/>
    public async Task UnsubscribeFromEventAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : EventMessage
    {       
        if (_consumer is null)
        {
            return;
        }

        _consumer.ReceivedAsync -= _messageHandler;
        _consumer = null;

        await DisconnectAsync(cancellationToken);

        Logger?.LogInformation("Subscription to '{eventName}' event has been removed.", typeof(TEvent).Name);
    }

    private async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_channel is not null && _channel.IsOpen)
        {
            await _channel.CloseAsync(cancellationToken: cancellationToken);
        }

        _channel = null;
    }
}
