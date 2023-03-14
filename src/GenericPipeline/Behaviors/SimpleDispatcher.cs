namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that dispatches a request to the associated request handler.
/// </summary>
/// <typeparam name="THandler">The type of the request handler to dispatch requests to.</typeparam>
public class SimpleDispatcher<THandler> : PipelineBehavior
    where THandler : IRequestHandler
{
    private readonly THandler _handler;
    private readonly HandlerOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleDispatcher{THandler}"/> class with the specified request handler and options.
    /// </summary>
    /// <param name="handler">The request handler to dispatch requests to.</param>
    /// <param name="options">The options for handling requests.</param>
    /// <exception cref="ArgumentException">Thrown when the provided handler is not a valid request handler.</exception>
    public SimpleDispatcher(THandler handler, HandlerOptions options)
    {
        if (handler is not IRequestHandler)
        {
            throw new ArgumentException(
                $"The provided handler {typeof(THandler).FullName} is not a valid request handler. " +
                $"Please ensure it implements the {nameof(IRequestHandler)} interface.",
                nameof(handler));
        }

        _handler = handler;
        _options = options;
    }

    /// <summary>
    /// Invokes the associated request handler to handle a given request.
    /// If the handler cannot handle the request and <see cref="HandlerOptions.ThrowUhandledRequestType"/> is true,
    /// an exception is thrown. Otherwise, the next behavior in the pipeline is invoked.
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
        else if (_options.ThrowUhandledRequestType)
        {
            // TODO proper exception?
            throw new InvalidOperationException(
                $"Handler '{typeof(THandler).FullName}' does not accept " +
                $"requests of type '{typeof(TRequest).FullName}' returning '{typeof(TResponse).FullName}'.");
        }
        else
        {
            return HandleNext<TRequest, TResponse>(request);
        }
    }
}


/// <summary>
/// Provides options for a pipeline behavior that invokes a request handler to handle a request.
/// </summary>
public sealed class HandlerOptions
{
    /// <summary>
    /// The default instance of the <see cref="HandlerOptions"/> class.
    /// </summary>
    public static readonly HandlerOptions Default = new()
    {
        ThrowUhandledRequestType = false
    };

    /// <summary>
    /// Gets or sets a value indicating whether an exception should be thrown when a handler cannot handle a request type.
    /// </summary>
    public bool ThrowUhandledRequestType;
}

