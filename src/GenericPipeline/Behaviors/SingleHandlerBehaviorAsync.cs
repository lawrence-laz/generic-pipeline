namespace GenericPipeline.Behaviors;

/// TODO
public class SingleHandlerBehaviorAsync<TRequestHandler> : PipelineBehaviorAsync
    where TRequestHandler : IRequestHandler
{
    internal readonly TRequestHandler _handler;

    /// TODO
    public SingleHandlerBehaviorAsync(TRequestHandler handler)
    {
        _handler = handler;
    }

    /// TODO
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        if (_handler is IRequestHandlerAsync<TRequest, TResponse> handlerAsync)
        {
            return await handlerAsync.Handle(request);
        }
        else if (_handler is IRequestHandler<TRequest, TResponse> handler)
        {
            return handler.Handle(request);
        }
        else
        {
            return await HandleNext<TRequest, TResponse>(request);
        }
    }
}

