//using Microsoft.Extensions.Logging;
//using SideEffect.Messaging.PubSub;
//using SideEffect.Messaging.RPC;

//namespace SideEffect.Messaging.Redis;

///// <summary>
///// Represents message producer with support of Pub/Sub and RPC flows.
///// </summary>
///// <param name="settings">See <see cref="ServiceBusSettings"/> for more information.</param>
///// <param name="logger">See <see cref="ILogger"/> for more information.</param>
//public class MessageProducer(ServiceBusSettings settings, ILogger<MessageProducer> logger)
//    : ServiceBusClientBase(settings, logger: logger), IMessageProducer
//{
//    /// <inheritdoc/>
//    public async Task PublishEventAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
//        where TEvent : Event
//    {
//        var channel = await GetChannelAsync<TEvent>(cancellationToken);
//        var value = await GetValueAsync(message, cancellationToken);
        
//        var connection = await GetConnectionAsync(cancellationToken);

//        var subscriber = connection.GetSubscriber();
//        await subscriber.PublishAsync(channel, value);

//        Logger?.LogInformation("Event of type '{eventName}' has been published.", typeof(TEvent).Name);
//    }

//    /// <inheritdoc/>
//    public Task<TResponse> ExecuteRequestAsync<TRequest, TResponse>(TRequest message, CancellationToken cancellationToken = default)
//        where TRequest : Request
//        where TResponse : Response
//    {
//        throw new NotImplementedException();
//    }
//}
