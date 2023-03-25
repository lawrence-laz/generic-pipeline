namespace GenericPipeline;

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
    public static Pipeline AppendBehavior<TBehavior>(this Pipeline pipeline)
        where TBehavior : PipelineBehavior, new()
        => pipeline.AppendBehavior(new TBehavior());

    /// <summary>
    /// Appends a handler of the specified type to the pipeline.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static Pipeline AppendHandler<THandler>(this Pipeline pipeline)
        where THandler : IRequestHandler, new()
        => pipeline.AppendHandler(new THandler());

    /// <summary>
    /// Appends a handler of the specified type to the pipeline.
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
            handler);
        return pipeline.AppendBehavior(behavior);
    }

    /// <summary>
    /// Appends a handler to the pipeline.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <param name="handler">The handler to add to the pipeline.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static Pipeline AppendHandler<THandler>(
        this Pipeline pipeline,
        THandler handler)
        where THandler : IRequestHandler
        => pipeline.AppendBehavior(new SingleHandlerBehavior<THandler>(handler));

    /// <summary>
    /// Sends a request through the pipeline and returns the response.
    /// This method is significantly slower than the generic variant, because
    /// reflection is inevitable here. Use it only when the generic method does 
    /// not suffice.
    /// </summary>
    /// <param name="pipeline">The pipeline instance to use.</param>
    /// <param name="request">The request to send.</param>
    /// <returns>The response of the request.</returns>
    public static object Send(
        this Pipeline pipeline,
        object request)
    {
        var requestType = request.GetType();
        var responseType = requestType
            .GetInterfaces()
            .First(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRequest<>))
            .GetGenericArguments()
            .First();
        var sendMethod = typeof(Pipeline)
            .GetMethods()
            .First(method => method.Name == nameof(Pipeline.Send) && method.GetGenericArguments().Length == 2)
            .MakeGenericMethod(requestType, responseType);
        var response = sendMethod.Invoke(pipeline, new[] { request });

        return response;
    }

    /// <summary>
    /// Adds a behavior to the pipeline that throws an exception if an unhandled request is encountered.
    /// </summary>
    /// <param name="pipeline">The pipeline to add the behavior to.</param>
    /// <returns>The pipeline with the added behavior.</returns>
    public static Pipeline ThrowOnUnhandledRequest(this Pipeline pipeline)
        => pipeline.AppendBehavior<UnhandledThrowingBehavior>();

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
        => pipeline.AppendBehavior(serviceProvider.GetBehavior<TBehavior>());

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
        where THandler : IRequestHandler
        => pipeline.AppendBehavior(new SingleHandlerBehavior<THandler>(serviceProvider.GetHandler<THandler>()));
}

