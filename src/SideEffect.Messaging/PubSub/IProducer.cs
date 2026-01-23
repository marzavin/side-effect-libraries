namespace SideEffect.Messaging.PubSub;

/// <summary>
/// Represents Pub/Sub message producer.
/// </summary>
public interface IProducer
{
    /// <summary>
    /// Publishes Pub/Sub event.
    /// </summary>
    /// <typeparam name="TEvent">Type of event message.</typeparam>
    /// <param name="message">See <see cref="Event"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task PublishEventAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : Event;
}
