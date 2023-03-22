namespace GenericPipeline;

/// TODO
public interface IRequestHandlerAsync
{
}

/// TODO
public interface IRequestHandlerAsync<TRequest, TResponse>
    : IRequestHandlerAsync
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

