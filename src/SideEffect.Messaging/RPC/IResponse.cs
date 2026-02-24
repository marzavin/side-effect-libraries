namespace SideEffect.Messaging.RPC;

/// <summary>
/// A base interface for all response messages.
/// </summary>
public interface IResponse : IMessage
{
    /// <summary>
    /// Gets or sets list of errors (see <see cref="ErrorModel"/>).
    /// </summary>
    public List<ErrorModel> Errors { get; set; }
}
