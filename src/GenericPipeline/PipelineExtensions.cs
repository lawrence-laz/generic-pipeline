namespace GenericPipeline;

/// TODO
public static class PipelineExtensions
{

    /// TODO
    public static Pipeline AppendBehavior<TBehavior>(this Pipeline pipeline) where TBehavior : PipelineBehavior
    {
        var behavior = Activator.CreateInstance<TBehavior>();
        return pipeline.AppendBehavior(behavior);
    }

    /// TODO
    public static Pipeline AppendHandler<THandler>(this Pipeline pipeline)
    {
        return pipeline.AppendHandler<THandler>(HandlerOptions.Default);
    }

    /// TODO
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        HandlerOptions options)
    {
        var handler = Activator.CreateInstance<THandler>();
        return pipeline.AppendBehavior(new SimpleDispatcher<THandler>(handler, options));
    }
}

