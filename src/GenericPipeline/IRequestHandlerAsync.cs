namespace GenericPipeline;

/// <summary>
/// Represents a handler for processing a request message asynchronously with a specified response type.
/// </summary>
/// <typeparam name="TRequest">The type of the request message.</typeparam>
/// <typeparam name="TResponse">The type of the response message.</typeparam>
public interface IRequestHandlerAsync<TRequest, TResponse>
    : IRequestHandler
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processes the specified request message asynchronously and returns a response message.
    /// </summary>
    /// <param name="request">The request message to process.</param>
    /// <param name="cancellationToken">The token to cancel the request with.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response message.</returns> 
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Represents a handler for processing a request message asynchronously with a default response type of <see cref="Unit"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request message.</typeparam>
public interface IRequestHandlerAsync<TRequest> : IRequestHandlerAsync<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
}
