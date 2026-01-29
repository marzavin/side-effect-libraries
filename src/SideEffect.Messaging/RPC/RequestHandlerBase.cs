namespace SideEffect.Messaging.RPC;

/// <summary>
/// Base class for request handlers.
/// </summary>
/// <typeparam name="TRequest">Type of request message.</typeparam>
/// <typeparam name="TResponse">Type of response message.</typeparam>
public abstract class RequestHandlerBase<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResponse, new()
{
    /// <summary>
    /// Handles incoming request message.
    /// </summary>
    /// <param name="message">See <see cref="IRequest{TResponse}"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="IResponse"/> for more information.</returns>
    public abstract Task<TResponse> HandleAsync(TRequest message, CancellationToken cancellationToken = default);
}
