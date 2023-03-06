namespace GenericPipeline;

/// TODO
public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// TODO
    TResponse Handle(TRequest request);
}

/// TODO
public interface IRequestHandler<TRequest> : IRequestHandler<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
}

