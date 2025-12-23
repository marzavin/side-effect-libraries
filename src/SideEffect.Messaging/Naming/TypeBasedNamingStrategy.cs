namespace SideEffect.Messaging.Naming;

/// <summary>
/// Implementation of <see cref="INamingStrategy"/> that uses message type as name source.
/// </summary>
public class TypeBasedNamingStrategy : INamingStrategy
{
    /// <inheritdoc/>
    public Task<string> GetChannelNameAsync<TMessage>(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(typeof(TMessage).Name);
    }
}
