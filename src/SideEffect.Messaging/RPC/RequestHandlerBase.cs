namespace SideEffect.Messaging.RPC;

/// <summary>
/// Base class for request handlers.
/// </summary>
/// <typeparam name="TRequest">Type of request message.</typeparam>
/// <typeparam name="TResponse">Type of response message.</typeparam>
public abstract class RequestHandlerBase<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response
{
    /// <summary>
    /// Handles incoming request message.
    /// </summary>
    /// <param name="message">See <see cref="Request"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Response"/> for more information.</returns>
    public abstract Task<TResponse> HandleAsync(TRequest message, CancellationToken cancellationToken = default);
}
