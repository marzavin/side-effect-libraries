namespace SideEffect.Messaging;

/// <summary>
/// Base event message.
/// </summary>
public abstract class EventMessage : IMessage;

/// <inheritdoc/>
public abstract class EventMessage<TData> : EventMessage
{
    /// <summary>
    /// Initializes a new instance of <see cref="EventMessage"/>.
    /// </summary>
    /// <param name="data">Message content.</param>
    public EventMessage(TData data)
    {
        Data = data;
    }

    /// <summary>
    /// Gets message content.
    /// </summary>
    public TData Data { get; private set; }
}