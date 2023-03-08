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
    {
        var handler = Activator.CreateInstance<THandler>();
        return pipeline.AppendBehavior(new SimpleDispatcherAsync<THandler>(handler));
    }
}

