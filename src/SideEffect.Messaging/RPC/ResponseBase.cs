namespace SideEffect.Messaging.RPC;

/// <summary>
/// A base class for response messages.
/// </summary>
public abstract class ResponseBase : IResponse
{
    /// <inheritdoc/>
    public List<ErrorModel> Errors { get; set; }
}
