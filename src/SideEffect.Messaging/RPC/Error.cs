namespace SideEffect.Messaging.RPC;

/// <summary>
/// Information about execution error.
/// </summary>
public class Error
{
    /// <summary>
    /// Gets or sets error key (provider).
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets error code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string Message { get; set; }
}
