namespace SideEffect.Messaging.RPC;

/// <summary>
/// Represents RPC message consumer.
/// </summary>
/// <typeparam name="TRequest">Type of request message.</typeparam>
public interface IConsumer<TRequest>
    where TRequest : Request
{
    /// <summary>
    /// Subscribes to request message.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task SubscribeToRequestAsync(CancellationToken cancellationToken = default);        

    /// <summary>
    /// Unsubscribes from request message.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task UnsubscribeFromRequestAsync(CancellationToken cancellationToken = default);
}
