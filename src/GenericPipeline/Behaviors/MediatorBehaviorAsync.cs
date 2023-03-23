using System.Collections.Concurrent;

namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that handles requests by dispatching them to the appropriate request handler.
/// </summary>
public class MediatorBehaviorAsync : PipelineBehaviorAsync
{
    internal ConcurrentDictionary<Type, object> _requestHandlers = new();

    /// <summary>
    /// Adds a new request handler of the specified type to the list of request handlers that can handle requests.
    /// </summary>
    /// <typeparam name="THandler">The type of the request handler to add.</typeparam>
    /// <returns>The updated mediator behavior instance.</returns>
    public MediatorBehaviorAsync AddHandler<THandler>()
        where THandler : IRequestHandler, new()
    {
        AddHandler(new THandler());
        return this;
    }

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
        foreach (var handlerType in requestHandlerInterfaces)
        {
            var requestType = handlerType.GetGenericArguments()[0];
            if (!_requestHandlers.TryAdd(requestType, handler))
            {
                throw new DuplicateHandlerException(
                    $"The request handler for type '{requestType.FullName}' " +
                    "has already been added to the mediator behavior instance.");
            }
        }

        return this;
    }

    /// <summary>
    /// Handles the specified request by dispatching it to the appropriate request handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to handle.</typeparam>
    /// <typeparam name="TResponse">The type of the response to return.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response of the handled request.</returns>
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        if (_requestHandlers.TryGetValue(request.GetType(), out var handler))
        {
            if (handler is IRequestHandlerAsync<TRequest, TResponse> requestHandlerAsync)
            {
                return await requestHandlerAsync.Handle(request);
            }
            else if (handler is IRequestHandler<TRequest, TResponse> requestHandler)
            {
                return requestHandler.Handle(request);
            }
        }

        return await HandleNext<TRequest, TResponse>(request);
    }
}

