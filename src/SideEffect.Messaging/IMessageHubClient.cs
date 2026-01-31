using SideEffect.Messaging.PubSub;
using SideEffect.Messaging.RPC;

namespace SideEffect.Messaging;

/// <summary>
/// An interface of client for sending messages to the messaging hub.
/// </summary>
public interface IMessageHubClient
{
    /// <summary>
    /// Publishes Publish/Subscribe event.
    /// </summary>
    /// <typeparam name="TEvent">Type of event message.</typeparam>
    /// <param name="message">See <see cref="IEvent"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    public Task PublishEventAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : IEvent;

    /// <summary>
    /// Executes RPC request.
    /// </summary>
    /// <typeparam name="TRequest">See <see cref="IRequest{TResponse}"/> for more information.</typeparam>
    /// <typeparam name="TResponse">See <see cref="IResponse"/> for more information.</typeparam>
    /// <param name="message">See <see cref="IRequest{TResponse}"/> for more information.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="IResponse"/> for more information.</returns>
    public Task<TResponse> ExecuteRequestAsync<TRequest, TResponse>(TRequest message, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
        where TResponse : IResponse, new();
}
