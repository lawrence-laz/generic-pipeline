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
        var handler = Activator.CreateInstance<THandler>();
        return pipeline.AppendBehavior(new SimpleDispatcher<THandler>(handler));
    }
}

