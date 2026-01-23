namespace SideEffect.Messaging.PubSub;

/// <summary>
/// Represents Pub/Sub message consumer.
/// </summary>
/// <typeparam name="TEvent">Type of event message.</typeparam>
public interface IConsumer<TEvent>
    where TEvent : Event
{
    /// <summary>
    /// Subscribes to event message.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task SubscribeToEventAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Unsubscribes from event message.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task UnsubscribeFromEventAsync(CancellationToken cancellationToken = default);
}
