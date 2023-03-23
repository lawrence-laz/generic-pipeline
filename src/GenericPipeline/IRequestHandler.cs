namespace GenericPipeline;

/// <summary>
/// Represents a handler for processing a request message.
/// </summary>
public interface IRequestHandler
{
}

/// <summary>
/// Represents a handler for processing a request message with a specified response type.
/// </summary>
/// <typeparam name="TRequest">The type of the request message.</typeparam>
/// <typeparam name="TResponse">The type of the response message.</typeparam>
public interface IRequestHandler<TRequest, out TResponse> : IRequestHandler
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processes the specified request message and returns a response message.
    /// </summary>
    /// <param name="request">The request message to process.</param>
    /// <returns>The response message.</returns> 
    TResponse Handle(TRequest request);
}

/// <summary>
/// Represents a handler for processing a request message with a default response type of <see cref="Unit"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request message.</typeparam>
public interface IRequestHandler<TRequest> : IRequestHandler<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
}

