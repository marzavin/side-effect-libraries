namespace SideEffect.Messaging.RPC;

/// <summary>
/// Represents RPC message producer.
/// </summary>
public interface IProducer
{
    /// <summary>
    /// Executes RPC request.
    /// </summary>
    /// <typeparam name="TRequest">Type of request message.</typeparam>
    /// <typeparam name="TResponse">Type of response message.</typeparam>
    /// <param name="message">See <see cref="Request"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Response"/> for more information.</returns>
    Task<TResponse> ExecuteRequestAsync<TRequest, TResponse>(TRequest message, CancellationToken cancellationToken = default)
        where TRequest : Request
        where TResponse : Response;
}
