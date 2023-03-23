namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that dispatches a request to the associated request handler.
/// </summary>
/// <typeparam name="THandler">The type of the request handler to dispatch requests to.</typeparam>
public class SingleHandlerBehavior<THandler> : PipelineBehavior
    where THandler : IRequestHandler
{
    internal readonly THandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleHandlerBehavior{THandler}"/> class with the specified request handler.
    /// </summary>
    /// <param name="handler">The request handler to dispatch requests to.</param>
    /// <exception cref="ArgumentException">Thrown when the provided handler is not a valid request handler.</exception>
    public SingleHandlerBehavior(THandler handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Invokes the associated request handler to handle a given request.
    /// If the handler cannot handle the request, then the next behavior in the pipeline is invoked.
    /// </summary>
    /// <typeparam name="TRequest">The type of request to handle.</typeparam>
    /// <typeparam name="TResponse">The type of response to return.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response of the handled request.</returns>
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        if (_handler is IRequestHandler<TRequest, TResponse> handler)
        {
            return handler.Handle(request);
        }
        else
        {
            return HandleNext<TRequest, TResponse>(request);
        }
    }
}

