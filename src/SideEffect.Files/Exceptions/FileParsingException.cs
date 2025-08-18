namespace SideEffect.Files.Exceptions;

/// <summary>
/// Base exception class for file parsing errors.
/// </summary>
public class FileParsingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileParsingException"/> class.
    /// </summary>
    public FileParsingException()
        : base()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileParsingException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public FileParsingException(string message)
        : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileParsingException"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public FileParsingException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
