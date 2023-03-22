namespace GenericPipeline;

/// TODO
public interface IRequestHandlerAsync<TRequest, TResponse>
    : IRequestHandler
    where TRequest : IRequest<TResponse>
{
    /// TODO
    Task<TResponse> Handle(TRequest request);
}

/// TODO
public interface IRequestHandlerAsync<TRequest> : IRequestHandlerAsync<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
}

