namespace GenericPipeline;

/// TODO
public static class PipelineAsyncExtensions
{
    /// TODO
    public static PipelineAsync AppendBehavior<TBehavior>(this PipelineAsync pipeline)
        where TBehavior : PipelineBehaviorAsync, new()
    {
        return pipeline.AppendBehavior(new TBehavior());
    }

    /// TODO
    public static PipelineAsync AppendHandler<THandler>(this PipelineAsync pipeline)
        where THandler : IRequestHandler, new()
    {
        return pipeline.AppendHandler<THandler>(new THandler());
    }

    /// TODO
    public static PipelineAsync AppendHandler<THandler>(
        this PipelineAsync pipeline,
        THandler handler)
        where THandler : IRequestHandler
    {
        return pipeline.AppendBehavior(new SingleHandlerBehaviorAsync<THandler>(handler));
    }

    /// TODO
    public static PipelineAsync ThrowOnUnhandledRequest(this PipelineAsync pipeline)
    {
        return pipeline.AppendBehavior<UnhandledThrowingBehaviorAsync>();
    }
}

