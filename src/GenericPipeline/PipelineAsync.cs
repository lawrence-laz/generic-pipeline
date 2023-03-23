namespace GenericPipeline;

/// <summary>
/// Represents a pipeline of behaviors that can be used to process requests of different types asynchronously.
/// </summary>
public sealed class PipelineAsync
{
    private PipelineBehaviorAsync? _firstBehavior;

    /// <summary>
    /// Appends a behavior to the end of the pipeline.
    /// </summary>
    /// <typeparam name="TBehavior">The type of the behavior to append.</typeparam>
    /// <param name="instance">The instance of the behavior to append.</param>
    /// <returns>The pipeline instance, to enable method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is null.</exception>
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
            GetLastBehavior(_firstBehavior).Next = instance;
        }

        return this;
    }

    /// <summary>
    /// Prepends a behavior to the beginning of the pipeline.
    /// </summary>
    /// <typeparam name="TBehavior">The type of the behavior to prepend.</typeparam>
    /// <param name="instance">The instance of the behavior to prepend.</param>
    /// <returns>The pipeline instance, to enable method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is null.</exception>
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


    /// <summary>
    /// Sends a request through the pipeline asynchronously and returns the response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <returns>The response of the request.</returns>
    /// <exception cref="Exception">Thrown when the pipeline has no behaviors.</exception>
    public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<TResponse>
    {
        if (_firstBehavior is null)
        {
            throw new InvalidOperationException("Cannot send the request. The pipeline does not have any behaviors attached.");
        }

        return await _firstBehavior.Handle<TRequest, TResponse>(request);
    }

    /// <summary>
    /// Sends a request through the pipeline asynchronously.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <returns>A unit value representing the completion of the request.</returns>
    /// <exception cref="Exception">Thrown when the pipeline has no behaviors.</exception>
    public Task<Unit> SendAsync<TRequest>(TRequest request)
        where TRequest : IRequest<Unit>
    {
        return SendAsync<TRequest, Unit>(request);
    }

    /// <summary>
    /// Gets the handler of the specified type from the pipeline.
    /// </summary>
    /// <typeparam name="THandler">The type of the handler to get.</typeparam>
    /// <returns>The handler of the specified type.</returns>
    /// <exception cref="HandlerNotFoundException">If the handler was not found in the pipeline.</exception>
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

    /// <summary>
    /// Gets the first behavior of the specified type in the pipeline.
    /// </summary>
    /// <typeparam name="TBehavior">The type of the behavior to get.</typeparam>
    /// <returns>The instance of the behavior.</returns>
    public TBehavior GetBehavior<TBehavior>() where TBehavior : PipelineBehaviorAsync
    {
        return (TBehavior)GetBehaviors().First(static behavior => behavior is TBehavior);
    }

    /// <summary>
    /// Gets an enumerable collection of all behaviors in the pipeline, in the order they are executed.
    /// </summary>
    /// <returns>An enumerable collection of behaviors in the pipeline.</returns>
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

    private PipelineBehaviorAsync GetLastBehavior(PipelineBehaviorAsync behavior)
    {
        var last = behavior;
        while (last.Next is not null)
        {
            last = last.Next;
        }

        return last;
    }
}

