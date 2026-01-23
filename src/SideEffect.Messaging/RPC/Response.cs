namespace SideEffect.Messaging.RPC;

/// <summary>
/// RPC response message.
/// </summary>
public class Response : MessageBase;

/// <summary>
/// RPC response message with payload.
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public sealed class Response<TPayload> : Response
{
    /// <summary>
    /// Initializes a new instance of <see cref="Response"/>.
    /// </summary>
    /// <param name="payload">Payload of message.</param>
    public Response(TPayload payload)
    {
        Payload = payload;
    }

    /// <summary>
    /// Gets message payload.
    /// </summary>
    public TPayload Payload { get; private set; }
}
