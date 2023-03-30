namespace GenericPipeline;

/// <summary>
/// Represents a behavior that can be used in an asynchronous pipeline.
/// </summary>
public abstract class PipelineBehaviorAsync
{
    /// <summary>
    /// Gets or sets the next behavior in the pipeline.
    /// </summary>
    public PipelineBehaviorAsync? Next { get; internal set; }

    /// <summary>
    /// Handles a request and returns the response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>The response to the request.</returns>
    public abstract Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>;

    /// <summary>
    /// Invokes the next behavior in the pipeline and returns its response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>The response to the request.</returns>
    protected Task<TResponse> HandleNext<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        if (Next is not null)
        {
            return Next.Handle<TRequest, TResponse>(request, cancellationToken);
        }
        else
        {
            return Task.FromResult<TResponse>(default!);
        }
    }
}

