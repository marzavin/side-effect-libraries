namespace SideEffect.Messaging.PubSub;

/// <summary>
/// Base class for event handlers.
/// </summary>
/// <typeparam name="TEvent">Type of event message.</typeparam>
public abstract class EventHandlerBase<TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Handles incoming event message.
    /// </summary>
    /// <param name="message">See <see cref="IEvent"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns></returns>
    public abstract Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
}
