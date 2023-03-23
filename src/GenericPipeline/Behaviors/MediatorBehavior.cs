using System.Collections.Concurrent;

namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that handles requests by dispatching them to the appropriate request handler.
/// </summary>
public class MediatorBehavior : PipelineBehavior
{
    internal ConcurrentDictionary<Type, object> _requestHandlers = new();

    /// <summary>
    /// Adds the specified handler to the list of request handlers that can handle requests.
    /// </summary>
    /// <param name="handler">The request handler to add.</param>
    /// <returns>The updated mediator behavior instance.</returns>
    public MediatorBehavior AddHandler(object handler)
    {
        var requestHandlerInterfaces = handler
            .GetType()
            .GetInterfaces()
            .Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
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

    // public MediatorBehavior AddHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
    //     where TRequest : IRequest<TResponse>
    // {
    //     // _requestHandlers.Add(handler);
    //     return this;
    // }
    //
    // public MediatorBehavior ReplaceHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
    //     where TRequest : IRequest<TResponse>
    // {
    //     // _requestHandlers.RemoveWhere(handler => handler is IRequestHandler<TRequest, TResponse>);
    //     AddHandler(handler);
    //     return this;
    // }

    /// <summary>
    /// Handles the specified request by dispatching it to the appropriate request handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to handle.</typeparam>
    /// <typeparam name="TResponse">The type of the response to return.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response of the handled request.</returns>
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        if (_requestHandlers.TryGetValue(request.GetType(), out var handler)
            && handler is IRequestHandler<TRequest, TResponse> requestHandler)
        {
            return requestHandler.Handle(request);
        }
        else
        {
            return HandleNext<TRequest, TResponse>(request);
        }
    }
}

