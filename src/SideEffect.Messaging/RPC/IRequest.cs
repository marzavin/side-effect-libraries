namespace SideEffect.Messaging.RPC;

/// <summary>
/// Base interface for request messages.
/// </summary>
public interface IRequest<TResponse> : IMessage
    where TResponse : IResponse;
