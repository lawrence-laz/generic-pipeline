namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that dispatches a request to the associated request handler asynchronously.
/// </summary>
/// <typeparam name="TRequestHandler">The type of the request handler to dispatch requests to.</typeparam>
public class SingleHandlerBehaviorAsync<TRequestHandler> : PipelineBehaviorAsync
    where TRequestHandler : IRequestHandler
{
    internal readonly TRequestHandler Handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleHandlerBehaviorAsync{TRequestHandler}"/> class with the specified request handler.
    /// </summary>
    /// <param name="handler">The request handler to dispatch requests to.</param>
    /// <exception cref="ArgumentException">Thrown when the provided handler is not a valid request handler.</exception>
    public SingleHandlerBehaviorAsync(TRequestHandler handler)
    {
        if (handler is null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        Handler = handler;
    }

    /// <summary>
    /// Invokes the associated request handler to handle a given request asynchronously.
    /// If the handler cannot handle the request, then the next behavior in the pipeline is invoked.
    /// </summary>
    /// <typeparam name="TRequest">The type of request to handle.</typeparam>
    /// <typeparam name="TResponse">The type of response to return.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response of the handled request.</returns>
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        if (Handler is IRequestHandlerAsync<TRequest, TResponse> handlerAsync)
        {
            return await handlerAsync.Handle(request).ConfigureAwait(false);
        }
        else if (Handler is IRequestHandler<TRequest, TResponse> handler)
        {
            return handler.Handle(request);
        }
        else
        {
            return await HandleNext<TRequest, TResponse>(request).ConfigureAwait(false);
        }
    }
}

