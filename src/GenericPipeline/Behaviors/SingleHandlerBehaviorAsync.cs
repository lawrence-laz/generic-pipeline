namespace GenericPipeline.Behaviors;

/// TODO
public class SingleHandlerBehaviorAsync<TRequestHandler> : PipelineBehaviorAsync
    where TRequestHandler : IRequestHandlerAsync
{
    private readonly TRequestHandler _handler;

    /// TODO
    public SingleHandlerBehaviorAsync(TRequestHandler handler)
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

/// TODO
public class SingleSyncHandlerBehaviorAsync<TRequestHandler> : PipelineBehaviorAsync
    where TRequestHandler : IRequestHandler
{
    private readonly TRequestHandler _handler;

    /// TODO
    public SingleSyncHandlerBehaviorAsync(TRequestHandler handler)
    {
        _handler = handler;
    }

    /// TODO
    public override Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        if (_handler is IRequestHandler<TRequest, TResponse> handler)
        {
            return Task.FromResult(handler.Handle(request));
        }
        else
        {
            throw new InvalidOperationException(
                $"Handler '{typeof(TRequestHandler).FullName}' does not accept" +
                $"requests of type '{typeof(TRequest).FullName}' returning '{typeof(TResponse).FullName}'.");
        }
    }
}

