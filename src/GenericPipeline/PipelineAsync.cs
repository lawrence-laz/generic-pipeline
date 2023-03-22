namespace GenericPipeline;

/// TODO
public sealed class PipelineAsync
{
    private PipelineBehaviorAsync? _firstBehavior;

    /// TODO
    public PipelineAsync AppendBehavior<TBehavior>(TBehavior instance)
        where TBehavior : PipelineBehaviorAsync
    {
        if (instance is null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        if (_firstBehavior is null)
        {
            _firstBehavior = instance;
        }
        else
        {
            GetLastBehavior().Next = instance;
        }

        return this;
    }

    /// TODO
    public PipelineAsync PrependBehavior<TBehavior>(TBehavior instance)
        where TBehavior : PipelineBehaviorAsync
    {
        if (instance is null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        if (_firstBehavior is null)
        {
            _firstBehavior = instance;
        }
        else
        {
            instance.Next = _firstBehavior;
            _firstBehavior = instance;
        }

        return this;
    }

    /// TODO
    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
    {
        if (_firstBehavior is null)
        {
            throw new System.Exception("TODO");
        }

        return await _firstBehavior.Handle<TRequest, TResponse>(request);
    }

    /// TODO
    public Task<Unit> SendAsync<TRequest>(TRequest request)
        where TRequest : IRequest<Unit>
    {
        return SendAsync<TRequest, Unit>(request);
    }

    /// TODO
    public THandler GetHandler<THandler>()
        where THandler : IRequestHandler
    {
        // TODO: GetHandlers method?
        var singleHandlerBehavior = GetBehaviors()
            .OfType<SingleHandlerBehaviorAsync<THandler>>()
            .FirstOrDefault();
        if (singleHandlerBehavior is not null)
        {
            return singleHandlerBehavior._handler;
        }

        // TODO
        // var requestHandlerFromMediator = GetBehaviors()
        //     .OfType<MediatorBehavior>()
        //     .SelectMany(mediator => mediator._requestHandlers)
        //     .OfType<THandler>()
        //     .FirstOrDefault();
        // if (requestHandlerFromMediator is not null)
        // {
        //     return requestHandlerFromMediator;
        // }

        throw new HandlerNotFoundException();
    }
    /// TODO
    public TBehavior GetBehavior<TBehavior>() where TBehavior : PipelineBehaviorAsync
    {
        return (TBehavior)GetBehaviors().First(static behavior => behavior is TBehavior);
    }

    /// TODO iterators allocate memory, could be implemented as a simple loop with static lambda
    public IEnumerable<PipelineBehaviorAsync> GetBehaviors()
    {
        if (_firstBehavior is null)
        {
            yield break;
        }

        var behavior = _firstBehavior;

        do
        {
            yield return behavior;
            behavior = behavior.Next;
        } while (behavior is not null);
    }

    private PipelineBehaviorAsync GetLastBehavior()
    {
        if (_firstBehavior is null)
        {
            throw new Exception("TODO");
        }

        var last = _firstBehavior;
        while (last.Next is not null)
        {
            last = last.Next;
        }

        return last;
    }
}

