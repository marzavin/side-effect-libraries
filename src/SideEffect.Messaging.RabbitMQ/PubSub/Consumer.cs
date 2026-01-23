using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ.PubSub;

/// <summary>
/// Pub/Sub event consumer.
/// </summary>
public class Consumer<TEvent, THandler> : ConsumerBase, IConsumer<TEvent>
    where TEvent : Event
    where THandler : EventHandlerBase<TEvent>
{
    /// <summary>
    /// Initializes a new instance of <see cref="Consumer{TEvent, THandler}"/>.
    /// </summary>
    /// <param name="settings">See <see cref="MessagingSettings"/> for more information.</param>
    /// <param name="serializer">See <see cref="IObjectSerializer"/> for more information.</param>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <param name="serviceProvider">See <see cref="IServiceProvider"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Consumer(
        MessagingSettings settings,
        IObjectSerializer serializer,
        ILogger<Consumer<TEvent, THandler>> logger,
        IServiceProvider serviceProvider)
        : base(settings, serializer, logger, serviceProvider)
    { }

    /// <summary>
    /// Establishes connections and subscribes to messages.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    public async Task SubscribeToEventAsync(CancellationToken cancellationToken = default)
    {
        if (Consumer is not null)
        {
            await UnsubscribeFromEventAsync(cancellationToken);
        }

        await ConnectAsync(cancellationToken);

        var exchangeName = typeof(TEvent).Name;
        var queueName = typeof(THandler).Name;

        await Channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await Channel.QueueDeclareAsync(
            queue: queueName,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await Channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: string.Empty,
            cancellationToken: cancellationToken);

        Handler = async (object sender, BasicDeliverEventArgs ea) =>
        {
            Logger?.LogInformation("Event of type '{messageType}' has been received.", typeof(TEvent).Name);

            try
            {
                using var scope = ServiceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<THandler>();

                var message = await Serializer.DeserializeObjectFromBytesAsync<TEvent>(ea.Body.ToArray());
                await handler.HandleAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "An error occured in '{handlerType}' during processing event {messageType}.", GetType().Name, typeof(TEvent).Name);
            }

            Logger?.LogInformation("Event of type '{messageType}' has been handled.", typeof(TEvent).Name);
        };

        Consumer = new AsyncEventingBasicConsumer(Channel);
        Consumer.ReceivedAsync += Handler;

        await Channel.BasicConsumeAsync(queueName, true, Consumer, cancellationToken: cancellationToken);

        Logger?.LogInformation("Subscription to '{messageType}' event has been added.", typeof(TEvent).Name);
    }

    /// <summary>
    /// Unsubscribes from messages and closes all connections.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    public async Task UnsubscribeFromEventAsync(CancellationToken cancellationToken = default)
    {
        await UnsubscribeAsync(cancellationToken);

        Logger?.LogInformation("Subscription to '{messageType}' event has been removed.", typeof(TEvent).Name);
    }
}
