namespace SideEffect.Messaging;

/// <summary>
/// Represents service bus message subscriber.
/// </summary>
public interface IServiceBusSubscriber
{
    /// <summary>
    /// Subscribes to service bus messages.
    /// </summary>
    /// <typeparam name="TEvent">Type of event message.</typeparam>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns></returns>
    Task SubscribeToEventAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : EventMessage;

    /// <summary>
    /// Unsubscribes from service bus messages.
    /// </summary>
    /// <typeparam name="TEvent">Type of event message.</typeparam>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns></returns>
    Task UnsubscribeFromEventAsync<TEvent>(CancellationToken cancellationToken = default)
        where TEvent : EventMessage;
}
