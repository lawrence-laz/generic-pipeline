namespace GenericPipeline;

// TODO: Add prepend overloads?

/// <summary>
/// Provides extension methods for the <see cref="Pipeline"/> class to add behaviors and handlers to the pipeline.
/// </summary>
public static class PipelineExtensions
{
    /// <summary>
    /// Appends a behavior of the specified type to the pipeline.
    /// </summary>
    /// <typeparam name="TBehavior">The type of the behavior to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the behavior to.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static Pipeline AppendBehavior<TBehavior>(this Pipeline pipeline) where TBehavior : PipelineBehavior
    {
        var behavior = Activator.CreateInstance<TBehavior>();
        return pipeline.AppendBehavior(behavior);
    }

    /// <summary>
    /// Appends a handler of the specified type to the pipeline with default options.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static Pipeline AppendHandler<THandler>(this Pipeline pipeline)
        where THandler : IRequestHandler
    {
        return pipeline.AppendHandler<THandler>(HandlerOptions.Default);
    }

    /// <summary>
    /// Appends a handler of the specified type to the pipeline with default options.
    /// </summary>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <param name="handlerType">The type of the handler to add.</param>
    /// <returns>The modified pipeline instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="handlerType"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="handlerType"/> is not a subclass of <see cref="IRequestHandler"/>.</exception>
    public static Pipeline AppendHandler(this Pipeline pipeline, Type handlerType)
    {
        if (handlerType is null)
        {
            throw new ArgumentNullException(nameof(handlerType));
        }

        if (!typeof(IRequestHandler).IsAssignableFrom(handlerType))
        {
            throw new ArgumentException(
                $"Type must be a subclass of {nameof(IRequestHandler)}",
                nameof(handlerType));
        }

        var handler = Activator.CreateInstance(handlerType);
        var behaviorType = typeof(SingleHandlerBehavior<>).MakeGenericType(handlerType);
        var behavior = (PipelineBehavior)Activator.CreateInstance(
            behaviorType,
            handler,
            HandlerOptions.Default);
        return pipeline.AppendBehavior(behavior);
    }

    /// <summary>
    /// Appends a handler of the specified type to the pipeline with the specified options.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <param name="options">The options to use when adding the handler.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        HandlerOptions options)
        where THandler : IRequestHandler
    {
        var handler = Activator.CreateInstance<THandler>();
        return pipeline.AppendHandler(handler, options);
    }

    // TODO: does this cause ambiguity for compiler if handler implements two handler interfaces?

    /// <summary>
    /// Appends a handler for the specified request and response types to the pipeline with default options.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <param name="handler">The handler to add to the pipeline.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        THandler handler)
        where THandler : IRequestHandler
    {
        return pipeline.AppendHandler(handler, HandlerOptions.Default);
    }

    /// <summary>
    /// Appends a handler of the specified type to the pipeline with the specified options.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <param name="handler">The handler to add to the pipeline.</param>
    /// <param name="options">The options to use when adding the handler.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        THandler handler,
        HandlerOptions options)
        where THandler : IRequestHandler
    {
        return pipeline.AppendBehavior(new SingleHandlerBehavior<THandler>(handler, options));
    }
}

