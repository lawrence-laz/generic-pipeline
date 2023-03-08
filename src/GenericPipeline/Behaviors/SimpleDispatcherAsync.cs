namespace GenericPipeline.Behaviors;

/// TODO
public class SimpleDispatcherAsync<TRequestHandler> : PipelineBehaviorAsync
{
    private readonly TRequestHandler _handler;

    /// TODO
    public SimpleDispatcherAsync(TRequestHandler handler)
    {
        _handler = handler;
    }

    /// TODO
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        if (_handler is IRequestHandlerAsync<TRequest, TResponse> handler)
        {
            return await handler.Handle(request);
        }
        else
        {
            throw new InvalidOperationException(
                $"Handler '{typeof(TRequestHandler).FullName}' does not accept" +
                $"requests of type '{typeof(TRequest).FullName}' returning '{typeof(TResponse).FullName}'.");
        }
    }
}

