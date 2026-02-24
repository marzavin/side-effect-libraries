namespace SideEffect.Messaging.RPC;

/// <summary>
/// A base interface for all request messages.
/// </summary>
public interface IRequest<TResponse> : IMessage
    where TResponse : IResponse;
