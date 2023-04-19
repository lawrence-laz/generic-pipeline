namespace GenericPipeline;

/// <summary>
/// Reperesents a request with a specified return type.
/// </summary>
/// <typeparam name="TResponse">The type of the request's response.</typeparam>
public interface IRequest<TResponse> : IBaseRequest
{
}

/// <summary>
/// Reperesents a request without a return type.
/// </summary>
public interface IRequest : IRequest<Unit>
{
}


/// TODO
public interface IBaseRequest
{
}
