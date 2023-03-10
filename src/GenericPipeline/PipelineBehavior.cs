namespace GenericPipeline;
/// TODO
public abstract class PipelineBehavior
{
    /// TODO
    public PipelineBehavior? Next { get; internal set; }

    /// TODO
    public abstract TResponse Handle<TRequest, TResponse>(
            TRequest request)
        where TRequest : IRequest<TResponse>;

    /// TODO
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

