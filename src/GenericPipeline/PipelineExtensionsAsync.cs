namespace GenericPipeline;

/// TODO
public static class PipelineAsyncExtensions
{
    /// TODO
    public static PipelineAsync AppendBehavior<TBehavior>(this PipelineAsync pipeline)
        where TBehavior : PipelineBehaviorAsync
    {
        var behavior = Activator.CreateInstance<TBehavior>();
        return pipeline.AppendBehavior(behavior);
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
}

