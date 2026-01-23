using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.RPC;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ.RPC;

/// <summary>
/// RPC consumer of messaging hub.
/// </summary>
public class Consumer<TRequest, TResponse, THandler> : ConsumerBase, IConsumer<TRequest>
    where TRequest : Request
    where TResponse : Response
    where THandler : RequestHandlerBase<TRequest, TResponse>
{
    /// <summary>
    /// Initializes a new instance of <see cref="Consumer{TRequest, TResponse, THandler}"/>.
    /// </summary>
    /// <param name="settings">See <see cref="MessagingSettings"/> for more information.</param>
    /// <param name="serializer">See <see cref="IObjectSerializer"/> for more information.</param>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <param name="serviceProvider">See <see cref="IServiceProvider"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public Consumer(
        MessagingSettings settings,
        IObjectSerializer serializer,
        ILogger<Consumer<TRequest, TResponse, THandler>> logger,
        IServiceProvider serviceProvider)
        : base(settings, serializer, logger, serviceProvider)
    { }

    /// <summary>
    /// Establishes connections and subscribes to messages.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    public async Task SubscribeToRequestAsync(CancellationToken cancellationToken = default)
    {
        if (Consumer is not null)
        {
            await UnsubscribeFromRequestAsync(cancellationToken);
        }

        await ConnectAsync(cancellationToken);

        var exchangeName = typeof(TRequest).Name;

        await Channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Direct,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var queueName = typeof(TResponse).Name;
        await Channel.QueueDeclareAsync(
            queue: queueName,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await Channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: queueName,
            cancellationToken: cancellationToken);

        Handler = async (object sender, BasicDeliverEventArgs ea) =>
        {
            var consumer = (AsyncEventingBasicConsumer)sender;
            var channel = consumer.Channel;
            var props = ea.BasicProperties;
            var messageProps = new BasicProperties { CorrelationId = props.CorrelationId };

            Logger?.LogInformation("Request of type '{messageType}' with correlation id='{correlationId}' has been received.", typeof(TRequest).Name, props.CorrelationId);

            TResponse response = null;

            try
            {
                using var scope = ServiceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<THandler>();

                var message = await Serializer.DeserializeObjectFromBytesAsync<TRequest>(ea.Body.ToArray());
                response = await handler.HandleAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "An error occured in '{handlerType}' during processing request {messageType}.", GetType().Name, typeof(TRequest).Name);

                var builder = new ResponseBuilder();
                builder.WithError(ex.Message);
                //response = builder.Build();
            }
            finally
            {
                var messageBody = await Serializer.SerializeObjectToBytesAsync(response);

                await channel.BasicPublishAsync(
                    exchange: exchangeName,
                    routingKey: props.ReplyTo,
                    mandatory: true,
                    basicProperties: messageProps,
                    body: messageBody,
                    cancellationToken: cancellationToken);

                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false);

                Logger?.LogInformation("Response of type '{messageType}' with correlation id='{correlationId}' has been sent.", typeof(TResponse).Name, props.CorrelationId);
            }
        };

        Consumer = new AsyncEventingBasicConsumer(Channel);
        Consumer.ReceivedAsync += Handler;

        await Channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: Consumer,
            cancellationToken: cancellationToken);

        Logger?.LogInformation("Subscription to '{messageType}' request has been added.", typeof(TRequest).FullName);
    }

    /// <summary>
    /// Unsubscribes from messages and closes all connections.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    public async Task UnsubscribeFromRequestAsync(CancellationToken cancellationToken = default)
    {
        await UnsubscribeAsync(cancellationToken);

        Logger?.LogInformation("Subscription to '{messageType}' request has been removed.", typeof(TRequest).FullName);
    }
}
