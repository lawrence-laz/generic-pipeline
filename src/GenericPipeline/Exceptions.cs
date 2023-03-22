namespace GenericPipeline;

/// <summary>
/// Represents an exception that is thrown when an error occurs in the pipeline.
/// </summary>
[System.Serializable]
public class GenericPipelineException : System.Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericPipelineException"/> class with default values.
    /// </summary>
    public GenericPipelineException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericPipelineException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public GenericPipelineException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericPipelineException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public GenericPipelineException(string message, System.Exception inner) : base(message, inner) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericPipelineException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected GenericPipelineException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

/// <summary>
/// Represents an exception that is thrown when the pipeline cannot handle a request.
/// </summary>
[System.Serializable]
public sealed class UnhandledRequestException : GenericPipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnhandledRequestException"/> class with default values.
    /// </summary>
    public UnhandledRequestException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnhandledRequestException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public UnhandledRequestException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnhandledRequestException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public UnhandledRequestException(string message, System.Exception inner) : base(message, inner) { }
}

/// <summary>
/// Represents an exception that is thrown when a duplicate handler is found in the pipeline.
/// </summary>
[System.Serializable]
public sealed class DuplicateHandlerException : GenericPipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateHandlerException"/> class with default values.
    /// </summary>
    public DuplicateHandlerException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateHandlerException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public DuplicateHandlerException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DuplicateHandlerException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public DuplicateHandlerException(string message, System.Exception inner) : base(message, inner) { }
}

/// TODO
[System.Serializable]
public sealed class HandlerNotFoundException : GenericPipelineException
{
    /// TODO
    public HandlerNotFoundException() { } // TODO: add message

    /// TODO
    public HandlerNotFoundException(string message) : base(message) { }

    /// TODO
    public HandlerNotFoundException(string message, System.Exception inner) : base(message, inner) { }
}

