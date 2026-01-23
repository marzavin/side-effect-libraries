using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SideEffect.Messaging.RPC;
using SideEffect.Messaging.Serialization;
using System.Collections.Concurrent;

namespace SideEffect.Messaging.RabbitMQ.RPC;

/// <summary>
/// RPC request producer.
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
    public async Task<TResponse> ExecuteRequestAsync<TRequest, TResponse>(TRequest message, CancellationToken cancellationToken)
        where TRequest : Request
        where TResponse : Response
    {
        await EstablishConnectionAsync(cancellationToken);

        using var channel = await Connection.CreateChannelAsync(cancellationToken: cancellationToken);

        ConcurrentDictionary<string, TaskCompletionSource<TResponse>> _callbackMapper = new();

        var exchangeName = typeof(TRequest).Name;

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Direct,
            autoDelete: false,
            cancellationToken: cancellationToken);

        var queueDeclareResult = await channel.QueueDeclareAsync(exclusive: false, cancellationToken: cancellationToken);
        var replyQueueName = queueDeclareResult.QueueName;

        await channel.QueueBindAsync(
            queue: replyQueueName,
            exchange: exchangeName,
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
        var messageProps = new BasicProperties { CorrelationId = correlationId, ReplyTo = replyQueueName};

        var tcs = new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
        _callbackMapper.TryAdd(correlationId, tcs);

        var messageBody = await Serializer.SerializeObjectToBytesAsync(message);

        var routingKey = typeof(TResponse).Name;
        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: messageProps,
            body: messageBody,
            cancellationToken: cancellationToken);

        Logger?.LogInformation("Request of type '{messageType}' with correlation id='{correlationId}' has been sent.", typeof(TRequest).Name, correlationId);

        using CancellationTokenRegistration ctr = cancellationToken.Register(() =>
        {
            _callbackMapper.TryRemove(correlationId, out _);
            tcs.SetCanceled();
        });

        return await tcs.Task;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await CloseConnectionAsync(CancellationToken.None);
    }
}
