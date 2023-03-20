namespace GenericPipeline;

/// <summary>
/// Represents a behavior that can be used in a pipeline.
/// </summary>
public abstract class PipelineBehavior
{
    /// <summary>
    /// Gets or sets the next behavior in the pipeline.
    /// </summary>
    public PipelineBehavior? Next { get; internal set; }

    /// <summary>
    /// Handles a request and returns the response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response to the request.</returns>
    public abstract TResponse Handle<TRequest, TResponse>(
            TRequest request)
        where TRequest : IRequest<TResponse>;

    /// <summary>
    /// Invokes the next behavior in the pipeline and returns its response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response to the request.</returns>
    protected TResponse HandleNext<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
    {
        if (Next is not null)
        {
            return Next.Handle<TRequest, TResponse>(request);
        }
        else
        {
            return default!; // TODO
        }
    }
}

