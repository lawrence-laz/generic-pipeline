namespace GenericPipeline;

/// TODO
public interface IRequestHandlerAsync<TRequest, TResponse>
    where TRequest : IRequest<Task<TResponse>>
{
    /// TODO
    Task<TResponse> Handle(TRequest request);
}

/// TODO
public interface IRequestHandlerAsync<TRequest> : IRequestHandlerAsync<TRequest, Unit>
    where TRequest : IRequest<Task<Unit>>
{
}

