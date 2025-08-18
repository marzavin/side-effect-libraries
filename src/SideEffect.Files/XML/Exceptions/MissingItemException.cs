using SideEffect.Files.Exceptions;

namespace SideEffect.Files.XML.Exceptions;

/// <summary>
/// Exception class for missing XML elements.
/// </summary>
public class MissingItemException : FileParsingException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingItemException"/> class.
    /// </summary>
    public MissingItemException()
        : base()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissingItemException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public MissingItemException(string message)
        : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MissingItemException"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public MissingItemException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
