namespace GenericPipeline;
/// TODO
public abstract class PipelineBehavior
{
    /// TODO
    private readonly PipelineBehavior next;

    /// TODO
    public PipelineBehavior(PipelineBehavior next)
    {
        this.next = next;
    }

    /// TODO
    public abstract TResponse Handle<TRequest, TResponse>(
            TRequest request)
        where TRequest : IRequest<TResponse>;

    /// TODO
    protected TResponse HandleNext<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
    {
        if (next is not null)
        {
            return next.Handle<TRequest, TResponse>(request);
        }
        else
        {
            return default!; // TODO
        }
    }
}

