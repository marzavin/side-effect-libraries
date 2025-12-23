namespace SideEffect.Messaging.Naming;

/// <summary>
/// Base interface for naming strategies.
/// </summary>
public interface INamingStrategy
{
    /// <summary>
    /// Creates name for communication channel.
    /// </summary>
    /// <typeparam name="TMessage">Type of service bus message.</typeparam>
    /// <param name="cancellationToken">See <see cref="CancellationToken"/> for more information.</param>
    /// <returns>Returns channel name.</returns>
    Task<string> GetChannelNameAsync<TMessage>(CancellationToken cancellationToken = default);
}
