namespace SideEffect.Messaging;

/// <summary>
/// Represents service bus message publisher.
/// </summary>
public interface IServiceBusPublisher
{
    /// <summary>
    /// Publishes event message (Pub/Sub Pattern).
    /// </summary>
    /// <typeparam name="TEvent">Message content type.</typeparam>
    /// <param name="message">See <see cref="EventMessage"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : EventMessage;
}
