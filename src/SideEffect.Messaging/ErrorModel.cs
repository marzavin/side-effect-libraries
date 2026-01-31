namespace SideEffect.Messaging;

/// <summary>
/// A model describing an execution error.
/// </summary>
public class ErrorModel
{
    /// <summary>
    /// Gets or sets error provider (source).
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// Gets or sets error code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string Message { get; set; }
}
