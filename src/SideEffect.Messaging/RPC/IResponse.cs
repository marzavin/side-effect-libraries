namespace SideEffect.Messaging.RPC;

/// <summary>
/// Base interface for response messages.
/// </summary>
public interface IResponse : IMessage
{
    public List<ErrorModel> Errors { get; set; }
}
