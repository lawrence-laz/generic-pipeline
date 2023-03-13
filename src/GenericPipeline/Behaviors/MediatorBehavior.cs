namespace GenericPipeline.Behaviors;

/// TODO
public class MediatorBehavior : PipelineBehavior
{
    private readonly HandlerOptions _handlerOptions;
    private Dictionary<Type, object> _requestHandlers = new();

    /// TODO
    public MediatorBehavior(HandlerOptions handlerOptions)
    {
        _handlerOptions = handlerOptions;
    }

    /// TODO
    public MediatorBehavior AddHandler(object handler)
    {
        var requestHandlerInterfaces = handler
            .GetType()
            .GetInterfaces()
            .Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
        foreach (var handlerType in requestHandlerInterfaces)
        {
            _requestHandlers.Add(handlerType.GetGenericArguments()[0], handler);
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

    /// TODO
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        if (_requestHandlers.TryGetValue(request.GetType(), out var handler)
            && handler is IRequestHandler<TRequest, TResponse> requestHandler)
        {
            return requestHandler.Handle(request);
        }
        else if (_handlerOptions.ThrowUhandledRequestType)
        {
            // TODO proper exception?
            throw new InvalidOperationException(
                $"Mediator does not accept requests of type " +
                $"'{typeof(TRequest).FullName}' returning '{typeof(TResponse).FullName}'.");
        }
        else
        {
            return HandleNext<TRequest, TResponse>(request);
        }
    }
}

