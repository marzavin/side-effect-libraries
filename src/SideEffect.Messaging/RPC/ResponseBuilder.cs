namespace SideEffect.Messaging.RPC;

/// <summary>
/// Builder for response message.
/// </summary>
public class ResponseBuilder
{
    private readonly List<Error> _errors = [];

    /// <summary>
    /// Builds and returns new instance of <see cref="Response"/>.
    /// </summary>
    /// <returns>See <see cref="Response"/>for more information.</returns>
    public virtual Response Build()
    {
        return new Response();
    }

    /// <summary>
    /// Adds error message to builder.
    /// </summary>
    /// <param name="error">See <see cref="Error"/> for more information.</param>
    /// <returns>See <see cref="ResponseBuilder"/> for more information.</returns>
    public ResponseBuilder WithError(Error error)
    {
        if (error is null)
        {
            throw new ArgumentNullException(nameof(error));
        }

        _errors.Add(error);

        return this;
    }

    /// <summary>
    /// Adds error message to builder.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <returns>See <see cref="ResponseBuilder"/> for more information.</returns>
    public ResponseBuilder WithError(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message));
        }

        _errors.Add(new Error { Message = message });

        return this;
    }
}

/// <summary>
/// Builder for response message with payload.
/// </summary>
public class ResponseBuilder<TPayload> : ResponseBuilder
{
    private TPayload _payload = default;

    /// <inheritdoc/>
    public override Response Build()
    {
        return new Response<TPayload>(_payload);
    }

    /// <summary>
    /// Adds message payload to builder.
    /// </summary>
    /// <param name="payload">Response payload.</param>
    /// <returns>See <see cref="ResponseBuilder"/> for more information.</returns>
    public ResponseBuilder WithPayload(TPayload payload)
    {
        _payload = payload;

        return this;
    }
}
