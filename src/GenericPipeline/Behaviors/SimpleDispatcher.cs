namespace GenericPipeline.Behaviors;

/// TODO
public class SimpleDispatcher<TRequestHandler> : FinalPipelineBehavior
{
    private readonly TRequestHandler _handler;

    /// TODO
    public SimpleDispatcher(TRequestHandler handler)
    {
        _handler = handler;
    }

    /// TODO
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        if (_handler is IRequestHandler<TRequest, TResponse> handler)
        {
            return handler.Handle(request);
        }
        else
        {
            throw new InvalidOperationException(
                $"Handler '{typeof(TRequestHandler).FullName}' does not accept" +
                $"requests of type '{typeof(TRequest).FullName}' returning '{typeof(TResponse).FullName}'.");
        }
    }
}

