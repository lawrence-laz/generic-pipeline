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
        where THandler : IRequestHandler
    {
        return pipeline.AppendHandler<THandler>(HandlerOptions.Default);
    }

    /// TODO
    public static Pipeline AppendHandler(this Pipeline pipeline, Type handlerType)
    {
        if (handlerType is null)
        {
            throw new ArgumentNullException(nameof(handlerType));
        }

        if (!handlerType.IsSubclassOf(typeof(IRequestHandler)))
        {
            throw new ArgumentException("TODO", nameof(handlerType));
        }

        var handler = Activator.CreateInstance(handlerType);
        var behaviorType = typeof(SingleHandlerBehavior<>).MakeGenericType(handlerType);
        var behavior = (PipelineBehavior)Activator.CreateInstance(
            behaviorType,
            handler,
            HandlerOptions.Default);
        return pipeline.AppendBehavior(behavior);
    }

    /// TODO
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        HandlerOptions options)
        where THandler : IRequestHandler
    {
        var handler = Activator.CreateInstance<THandler>();
        return pipeline.AppendHandler(handler, options);
    }

    /// TODO
    public static Pipeline AppendHandler<TRequest, TResponse>(
        this Pipeline pipeline,
        IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        return pipeline.AppendHandler(handler, HandlerOptions.Default);
    }

    /// TODO
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        THandler handler,
        HandlerOptions options)
        where THandler : IRequestHandler
    {
        return pipeline.AppendBehavior(new SingleHandlerBehavior<THandler>(handler, options));
    }
}

