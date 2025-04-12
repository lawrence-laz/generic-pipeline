namespace GenericPipeline;

/// <summary>
/// Provides extension methods for the <see cref="PipelineAsync"/> class to add behaviors and handlers to the pipeline.
/// </summary>
public static class PipelineAsyncExtensions
{
    /// <summary>
    /// Appends a behavior of the specified type to the pipeline.
    /// </summary>
    /// <typeparam name="TBehavior">The type of the behavior to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the behavior to.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static PipelineAsync AppendBehavior<TBehavior>(this PipelineAsync pipeline)
        where TBehavior : PipelineBehaviorAsync, new()
        => pipeline.AppendBehavior(new TBehavior());

    /// <summary>
    /// Appends a handler of the specified type to the pipeline.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to add.</typeparam>
    /// <param name="pipeline">The pipeline to add the handler to.</param>
    /// <returns>The modified pipeline instance.</returns>
    public static PipelineAsync AppendHandler<THandler>(this PipelineAsync pipeline)
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
    public static PipelineAsync AppendHandler(this PipelineAsync pipeline, Type handlerType)
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
        var behaviorType = typeof(SingleHandlerBehaviorAsync<>).MakeGenericType(handlerType);
        var behavior = (PipelineBehaviorAsync)Activator.CreateInstance(
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
    public static PipelineAsync AppendHandler<THandler>(
        this PipelineAsync pipeline,
        THandler handler)
        where THandler : IRequestHandler
        => pipeline.AppendBehavior(new SingleHandlerBehaviorAsync<THandler>(handler));

    /// <summary>
    /// Adds a behavior to the pipeline that throws an exception if an unhandled request is encountered.
    /// </summary>
    /// <param name="pipeline">The pipeline to add the behavior to.</param>
    /// <returns>The pipeline with the added behavior.</returns>
    public static PipelineAsync ThrowOnUnhandledRequest(this PipelineAsync pipeline)
        => pipeline.AppendBehavior<UnhandledThrowingBehaviorAsync>();

    /// <summary>
    /// Sends a request through the pipeline and returns the response.
    /// This method is significantly slower than the generic variant, because
    /// reflection is inevitable here. Use it only when the generic method does 
    /// not suffice.
    /// </summary>
    /// <param name="pipeline">The pipeline instance to use.</param>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">The token to cancel the request with.</param>
    /// <returns>The response of the request.</returns>
    public static async Task<object> SendAsync(
        this PipelineAsync pipeline,
        object request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        var requestType = request.GetType();
        var responseType = requestType
            .GetInterfaces()
            .FirstOrDefault(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRequest<>))
            ?.GetGenericArguments()
            .FirstOrDefault();
        if (responseType is null)
        {
            throw new ArgumentException(
                $"Request of type '{request.GetType().Name}' does not implement '{nameof(IRequest)}' interface.",
                nameof(request));
        }
        var sendMethod = typeof(PipelineAsync)
            .GetMethods()
            .First(method => method.Name == nameof(PipelineAsync.SendAsync) && method.GetGenericArguments().Length == 2)
            .MakeGenericMethod(requestType, responseType);
        var task = (Task)sendMethod.Invoke(pipeline, new[] { request, cancellationToken });
        await task.ConfigureAwait(false);
        var resultProperty = typeof(Task<>).MakeGenericType(responseType).GetProperty(nameof(Task<object>.Result));
        var result = resultProperty.GetValue(task);
        return result;
    }
}
