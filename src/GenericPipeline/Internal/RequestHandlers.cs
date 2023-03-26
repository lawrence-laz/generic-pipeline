using System.Collections.Concurrent;

namespace GenericPipeline.Internal;

internal sealed class RequestHandlers
{
    internal ConcurrentDictionary<Type, object> Handlers = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(Type handlerInterfaceType, object handler)
    {
        var requestType = handlerInterfaceType.GetGenericArguments()[0];
        if (!Handlers.TryAdd(requestType, handler))
        {
            throw new DuplicateHandlerException(
                $"The request handler for type '{requestType.FullName}' " +
                "has already been added to the mediator behavior instance.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(Type requestType, out object requestHandler)
        => Handlers.TryGetValue(requestType, out requestHandler);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<TRequest>(out object requestHandler)
        => TryGet(typeof(TRequest), out requestHandler);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveByRequestType(Type requestType)
        => Handlers.TryRemove(requestType, out _);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveByHandlerType(Type handlerType)
        => Handlers
            .Where(pair => pair.Value.GetType() == handlerType)
            .ToList()
            .ForEach(pair => RemoveByRequestType(pair.Key));
}


