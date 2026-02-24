namespace SideEffect.Messaging;

/// <summary>
/// Represents options for message hub.
/// </summary>
public class MessageHubOptions
{
    /// <summary>
    /// Gets handler registry.
    /// </summary>
    public IHandlerRegistry Registry { get; }

    /// <summary>
    /// Creates new instance of <see cref="MessageHubOptions"/>.
    /// </summary>
    public MessageHubOptions()
    {
        Registry = new HandlerRegistry();
    }
}
