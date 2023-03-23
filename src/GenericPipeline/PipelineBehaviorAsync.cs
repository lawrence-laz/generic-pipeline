namespace GenericPipeline;
/// TODO
public abstract class PipelineBehaviorAsync
{
    /// TODO
    public PipelineBehaviorAsync? Next { get; internal set; }

    /// TODO
    public abstract Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>;

    /// TODO
    protected Task<TResponse> HandleNext<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
    {
        if (Next is not null)
        {
            return Next.Handle<TRequest, TResponse>(request);
        }
        else
        {
            return Task.FromResult<TResponse>(default!);
        }
    }
}

