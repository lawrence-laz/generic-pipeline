namespace GenericPipeline.Extensions.DependencyInjection;

public static class PipelineExtensions
{
    /// TODO
    public static Pipeline AppendBehavior<TBehavior>(
        this Pipeline pipeline,
        IServiceProvider serviceProvider)
        where TBehavior : PipelineBehavior
    {
        var behavior = serviceProvider.GetService(typeof(TBehavior));
        if (behavior is null)
        {
            throw new Exception("TODO");
        }

        return pipeline.AppendBehavior((TBehavior)behavior);
    }

    /// TODO
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        IServiceProvider serviceProvider)
    {
        var handler = serviceProvider.GetService(typeof(THandler));
        if (handler is null)
        {
            throw new Exception("TODO");
        }

        return pipeline.AppendBehavior(new SimpleDispatcher<THandler>((THandler)handler));
    }
}

