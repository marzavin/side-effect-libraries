namespace SideEffect.Messaging;

/// <summary>
/// Base interface for service bus.
/// </summary>
public interface IServiceBus
{
    /// <summary>
    /// Adds new message handler for message type.
    /// </summary>
    /// <param name="handler">Message handler.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task SubscribeToEventAsync<TMessage>(Action<TMessage> handler, CancellationToken cancellationToken = default)
        where TMessage: IMessage;

    /// <summary>
    /// Adds new message handler for message type.
    /// </summary>
    /// <param name="handler">Async message handler.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task SubscribeToEventAsync<TMessage>(Func<TMessage, CancellationToken, Task> handler, CancellationToken cancellationToken = default)
        where TMessage : IMessage;

    /// <summary>
    /// Removes message handler for message type.
    /// </summary>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task UnsubscribeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes new message to channel.
    /// </summary>
    /// <typeparam name="TMessage">See <see cref="IMessage"/> for more information.</typeparam>
    /// <param name="message">Content for the message.</param>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>See <see cref="Task"/> for more information.</returns>
    Task PublishEventAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
         where TMessage : IMessage;
}
