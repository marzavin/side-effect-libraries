namespace SideEffect.Messaging.PubSub;

/// <summary>
/// Event message.
/// </summary>
public class Event : MessageBase;

/// <summary>
/// Event message with payload.
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public class Event<TPayload> : Event
{
    /// <summary>
    /// Initializes a new instance of <see cref="Event"/>.
    /// </summary>
    /// <param name="payload">Payload of message.</param>
    public Event(TPayload payload)
    {
        Payload = payload;
    }

    /// <summary>
    /// Gets message payload.
    /// </summary>
    public TPayload Payload { get; private set; }
}