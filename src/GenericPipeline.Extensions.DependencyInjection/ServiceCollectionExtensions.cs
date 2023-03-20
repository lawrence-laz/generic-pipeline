namespace GenericPipeline.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods for configuring a pipeline with behaviors and handlers using an <see cref="IServiceProvider"/>.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    /// Appends a behavior of type <typeparamref name="TBehavior"/> to the end of the pipeline.
    /// </summary>
    /// <typeparam name="TBehavior">The type of behavior to append.</typeparam>
    /// <param name="pipeline">The pipeline to append the behavior to.</param>
    /// <param name="serviceProvider">The service provider used to resolve the behavior instance.</param>
    /// <returns>The pipeline with the appended behavior.</returns>
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

    /// <summary>
    /// Appends a handler of type <typeparamref name="THandler"/> to the end of the pipeline.
    /// </summary>
    /// <typeparam name="THandler">The type of handler to append.</typeparam>
    /// <param name="pipeline">The pipeline to append the handler to.</param>
    /// <param name="serviceProvider">The service provider used to resolve the handler instance.</param>
    /// <returns>The pipeline with the appended handler.</returns>
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

