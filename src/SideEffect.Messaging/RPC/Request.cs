namespace SideEffect.Messaging.RPC;

/// <summary>
/// RPC request message.
/// </summary>
public class Request : MessageBase;

/// <summary>
/// RPC request message with payload.
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public sealed class Request<TPayload> : Request
{
    /// <summary>
    /// Initializes a new instance of <see cref="Request"/>.
    /// </summary>
    /// <param name="payload">Payload of message.</param>
    public Request(TPayload payload)
    {
        Payload = payload;
    }

    /// <summary>
    /// Gets message payload.
    /// </summary>
    public TPayload Payload { get; private set; }
}
