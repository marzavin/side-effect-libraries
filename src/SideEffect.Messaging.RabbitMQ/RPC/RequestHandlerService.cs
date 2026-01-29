using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.RPC;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ.RPC;

internal class RequestHandlerService<TRequest, TResponse, THandler> : IHostedService, IAsyncDisposable
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse, new()
    where THandler : RequestHandlerBase<TRequest, TResponse>
{
    public const string RemoteProcedureCallExchange = "RPC";

    protected IConnection Connection { get; set; }

    protected AsyncEventingBasicConsumer Consumer { get; set; }

    protected AsyncEventHandler<BasicDeliverEventArgs> Handler { get; set; }

    protected IChannel Channel { get; set; }

    /// <summary>
    /// Gets message hub settings.
    /// </summary>
    public MessageHubSettings Settings { get; }

    /// <summary>
    /// Gets message serializer.
    /// </summary>
    protected IObjectSerializer Serializer { get; }

    /// <summary>
    /// Gets provider for external dependencies.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger Logger { get; }

    public RequestHandlerService(
        MessageHubSettings settings, 
        IObjectSerializer serializer,
        IServiceProvider serviceProvider, 
        ILogger<RequestHandlerService<TRequest, TResponse, THandler>> logger)
    {
        Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region IHostedService implementation

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await SubscribeAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await UnsubscribeAsync(cancellationToken);
    }

    #endregion

    #region IAsyncDisposable implementation

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await UnsubscribeAsync(CancellationToken.None);
    }

    #endregion

    private async Task SubscribeAsync(CancellationToken cancellationToken = default)
    {
        await EstablishConnectionAsync(cancellationToken);

        if (Consumer is not null)
        {
            await UnsubscribeAsync(cancellationToken);
        }

        Channel = await Connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await Channel.ExchangeDeclareAsync(
            exchange: RemoteProcedureCallExchange,
            type: ExchangeType.Direct,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var queueName = GetQueueName();
        await Channel.QueueDeclareAsync(
            queue: queueName,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await Channel.QueueBindAsync(
            queue: queueName,
            exchange: RemoteProcedureCallExchange,
            routingKey: queueName,
            cancellationToken: cancellationToken);

        Handler = async (object sender, BasicDeliverEventArgs ea) =>
        {
            var consumer = (AsyncEventingBasicConsumer)sender;
            var channel = consumer.Channel;
            var props = ea.BasicProperties;
            var messageProps = new BasicProperties { CorrelationId = props.CorrelationId };

            Logger?.LogInformation("Request of type '{messageType}' with correlation id='{correlationId}' has been received.", typeof(TRequest).Name, props.CorrelationId);

            var reply = new TResponse();

            try
            {
                using var scope = ServiceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<THandler>();

                var message = await Serializer.DeserializeObjectFromBytesAsync<TRequest>(ea.Body.ToArray());
                reply = await handler.HandleAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "An error occured in service bus message handler '{handlerType}'.", GetType().Name);
                reply.Errors = [new() { Message = ex.Message }];
            }
            finally
            {
                var messageBody = await Serializer.SerializeObjectToBytesAsync(reply);

                await channel.BasicPublishAsync(
                    exchange: RemoteProcedureCallExchange,
                    routingKey: props.ReplyTo,
                    mandatory: true,
                    basicProperties: messageProps,
                    body: messageBody,
                    cancellationToken: cancellationToken);

                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false);

                Logger?.LogInformation("Reply of type '{messageType}' with correlation id='{correlationId}' has been sent.", typeof(TResponse).Name, props.CorrelationId);
            }
        };

        Consumer = new AsyncEventingBasicConsumer(Channel);
        Consumer.ReceivedAsync += Handler;

        await Channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: Consumer,
            cancellationToken: cancellationToken);
    }

    private async Task UnsubscribeAsync(CancellationToken cancellationToken = default)
    {
        if (Consumer is null)
        {
            return;
        }

        Consumer.ReceivedAsync -= Handler;
        Consumer = null;

        if (Channel is not null && Channel.IsOpen)
        {
            await Channel.CloseAsync(cancellationToken: cancellationToken);
        }

        Channel = null;

        await CloseConnectionAsync(cancellationToken);
    }

    /// <summary>
    /// Establishes connection with message hub.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    protected async Task EstablishConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (Connection is not null)
        {
            return;
        }

        var connectionFactory = new ConnectionFactory { Uri = new Uri(Settings.ConnectionString) };
        Connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
    }

    /// <summary>
    /// Closes connection with message hub.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    protected async Task CloseConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (Connection is null)
        {
            return;
        }

        await Connection.CloseAsync(cancellationToken);
        Connection = null;
    }

    private static string GetQueueName()
    {
        return typeof(TRequest).Name;
    }
}
