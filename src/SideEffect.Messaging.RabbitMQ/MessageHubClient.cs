using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.RPC;
using SideEffect.Messaging.Serialization;
using System.Collections.Concurrent;

namespace SideEffect.Messaging.RabbitMQ;

/// <inheritdoc/>
internal class MessageHubClient : IMessageHubClient, IAsyncDisposable
{
    public const string RemoteProcedureCallExchange = "RPC";

    /// <summary>
    /// Gets message hub settings.
    /// </summary>
    public MessageHubSettings Settings { get; }

    /// <summary>
    /// Gets message serializer.
    /// </summary>
    protected IObjectSerializer Serializer { get; }

    /// <summary>
    /// Gets logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets session.
    /// </summary>
    protected IConnection Connection { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="MessageHubClient"/>.
    /// </summary>
    /// <param name="logger">See <see cref="ILogger"/> for more information.</param>
    /// <param name="serializer">See <see cref="IObjectSerializer"/> for more information.</param>
    /// <param name="settings">See <see cref="MessageHubSettings"/> for more information.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public MessageHubClient(MessageHubSettings settings, IObjectSerializer serializer, ILogger<MessageHubClient> logger)
    {
        Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<TResponse> ExecuteRequestAsync<TRequest, TResponse>(TRequest message, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse, new()
    {
        await EstablishConnectionAsync(cancellationToken);

        using var channel = await Connection.CreateChannelAsync(cancellationToken: cancellationToken);

        ConcurrentDictionary<string, TaskCompletionSource<TResponse>> _callbackMapper = new();

        await channel.ExchangeDeclareAsync(
            exchange: RemoteProcedureCallExchange,
            type: ExchangeType.Direct,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var queueDeclareResult = await channel.QueueDeclareAsync(exclusive: false, cancellationToken: cancellationToken);
        var replyQueueName = queueDeclareResult.QueueName;

        await channel.QueueBindAsync(
            queue: replyQueueName,
            exchange: RemoteProcedureCallExchange,
            routingKey: replyQueueName,
            cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var correlationId = ea.BasicProperties.CorrelationId;

            if (!string.IsNullOrWhiteSpace(correlationId) && _callbackMapper.TryRemove(correlationId, out var tcs))
            {
                var reply = await Serializer.DeserializeObjectFromBytesAsync<TResponse>(ea.Body.ToArray());
                tcs.TrySetResult(reply);
            }

            Logger?.LogInformation("Response of type '{messageType}' with correlation id='{correlationId}' has been received.", typeof(TResponse).Name, correlationId);
        };

        await channel.BasicConsumeAsync(
            queue: replyQueueName,
            autoAck: true,
            consumer: consumer,
            cancellationToken: cancellationToken);

        var correlationId = Guid.NewGuid().ToString();
        var messageProps = new BasicProperties { CorrelationId = correlationId, ReplyTo = replyQueueName };

        var tcs = new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
        _callbackMapper.TryAdd(correlationId, tcs);

        var messageBody = await Serializer.SerializeObjectToBytesAsync(message);

        var routingKey = typeof(TRequest).Name;

        await channel.BasicPublishAsync(
            exchange: RemoteProcedureCallExchange,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: messageProps,
            body: messageBody,
            cancellationToken: cancellationToken);

        Logger?.LogInformation("Request of type '{messageType}' with correlation id='{correlationId}' has been sent.", typeof(TRequest).Name, correlationId);

        using var ctr = cancellationToken.Register(() =>
        {
            _callbackMapper.TryRemove(correlationId, out _);
            tcs.SetCanceled();
        });

        return await tcs.Task;
    }

    /// <inheritdoc/>
    public async Task PublishEventAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : IEvent
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

    #region IAsyncDisposable implementation

    public async ValueTask DisposeAsync()
    {
        await CloseConnectionAsync(CancellationToken.None);
    }

    #endregion
}
