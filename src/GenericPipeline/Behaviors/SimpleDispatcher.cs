namespace GenericPipeline.Behaviors;

/// TODO
public class SimpleDispatcher<THandler> : PipelineBehavior
    where THandler : IRequestHandler
{
    private readonly THandler _handler;
    private readonly HandlerOptions _options;

    /// TODO
    public SimpleDispatcher(THandler handler, HandlerOptions options)
    {
        if (handler is not IRequestHandler)
        {
            throw new ArgumentException("TODO", nameof(handler));
        }

        _handler = handler;
        _options = options;
    }

    /// TODO
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
/// Options for the <see cref="SimpleDispatcher{THandler}"/> class.
/// </summary>
public sealed class HandlerOptions
{
    /// TODO
    public static readonly HandlerOptions Default = new()
    {
        ThrowUhandledRequestType = false
    };

    /// TODO
    public bool ThrowUhandledRequestType;
}

