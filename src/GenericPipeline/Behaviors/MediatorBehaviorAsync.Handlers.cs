namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that handles requests by dispatching them to the appropriate request handler.
/// </summary>
public partial class MediatorBehaviorAsync : PipelineBehaviorAsync
{
    internal RequestHandlers Handlers = new();

    /// <summary>
    /// Adds a new request handler of the specified type to the list of request handlers that can handle requests.
    /// </summary>
    /// <typeparam name="THandler">The type of the request handler to add.</typeparam>
    /// <returns>The updated mediator behavior instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MediatorBehaviorAsync AddHandler<THandler>()
        where THandler : IRequestHandler, new()
        => AddHandler(new THandler());

    /// <summary>
    /// Adds the specified handler to the list of request handlers that can handle requests.
    /// </summary>
    /// <param name="handler">The request handler to add.</param>
    /// <returns>The updated mediator behavior instance.</returns>
    public MediatorBehaviorAsync AddHandler(object handler)
    {
        var requestHandlerInterfaces = handler
            .GetType()
            .GetInterfaces()
            .Where(type => type.IsGenericType && (
                type.GetGenericTypeDefinition() == typeof(IRequestHandlerAsync<,>)
                || type.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));
        foreach (var handlerInterfaceType in requestHandlerInterfaces)
        {
            Handlers.Add(handlerInterfaceType, handler);
        }

        return this;
    }

    /// <summary>
    /// Removes the request handler with the specified type from the list of request handlers that can handle requests.
    /// </summary>
    /// <param name="handlerType">The type of the request handler to remove.</param>
    /// <returns>The updated mediator behavior instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MediatorBehaviorAsync RemoveHandlerByHandlerType(Type handlerType)
    {
        Handlers.RemoveByHandlerType(handlerType);
        return this;
    }

    /// <summary>
    /// Removes the request handler with the specified type from the list of request handlers.
    /// </summary>
    /// <typeparam name="THandler">The type of the request handler to remove.</typeparam>
    /// <returns>The updated mediator behavior instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MediatorBehaviorAsync RemoveHandlerByHandlerType<THandler>()
        where THandler : IRequestHandler
        => RemoveHandlerByHandlerType(typeof(THandler));

    /// <summary>
    /// Removes the request handler with the specified request type from the list of request handlers.
    /// </summary>
    /// <param name="requestType">The type of the request handler to remove.</param>
    /// <returns>The updated mediator behavior instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MediatorBehaviorAsync RemoveHandlerByRequestType(Type requestType)
    {
        Handlers.RemoveByRequestType(requestType);
        return this;
    }

    /// <summary>
    /// Removes the request handler with the specified request type from the list of request handlers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to remove.</typeparam>
    /// <typeparam name="TResponse">The type of the request's response.</typeparam>
    /// <returns>The updated mediator behavior instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MediatorBehaviorAsync RemoveHandlerByRequestType<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
        => RemoveHandlerByRequestType(typeof(TRequest));

    /// <summary>
    /// Removes the request handler with the specified request type from the list of request handlers.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to remove.</typeparam>
    /// <returns>The updated mediator behavior instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MediatorBehaviorAsync RemoveHandlerByRequestType<TRequest>()
        where TRequest : IRequest
        => RemoveHandlerByRequestType(typeof(TRequest));
}
