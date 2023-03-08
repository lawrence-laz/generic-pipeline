namespace GenericPipeline;
/// TODO
public abstract class PipelineBehaviorAsync
{
    /// TODO
    public PipelineBehaviorAsync? Next { get; internal set; }

    /// TODO
    public abstract Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<Task<TResponse>>;

    /// TODO
    protected Task<TResponse> HandleNext<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<Task<TResponse>>
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

