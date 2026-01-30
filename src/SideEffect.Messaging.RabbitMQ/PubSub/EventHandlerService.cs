using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.Serialization;

namespace SideEffect.Messaging.RabbitMQ.PubSub;

internal class EventHandlerService<TEvent, THandler> : IHostedService, IAsyncDisposable
    where TEvent : IEvent
    where THandler : EventHandlerBase<TEvent>
{
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

    public EventHandlerService(
        MessageHubSettings settings,
        IObjectSerializer serializer,
        IServiceProvider serviceProvider,
        ILogger<EventHandlerService<TEvent, THandler>> logger)
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

        Logger?.LogInformation("Publish/Subscribe handler '{handlerType}' has been added.", typeof(THandler).Name);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await UnsubscribeAsync(cancellationToken);

        Logger?.LogInformation("Publish/Subscribe handler '{handlerType}' has been removed.", typeof(THandler).Name);
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

        var exchangeName = GetExchangeName();
        await Channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Fanout,
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
                Logger?.LogError(ex, "An error occured in service bus message handler '{handler}'.", GetType());
            }

            Logger?.LogInformation("Event of type '{messageType}' has been handled.", typeof(TEvent).Name);
        };

        Consumer = new AsyncEventingBasicConsumer(Channel);
        Consumer.ReceivedAsync += Handler;

        await Channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,
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

    private static string GetExchangeName()
    {
        return typeof(TEvent).Name;
    }

    private static string GetQueueName()
    {
        return typeof(THandler).Name;
    }
}
