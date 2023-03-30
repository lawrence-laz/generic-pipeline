namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that handles requests by dispatching them to the appropriate request handler.
/// </summary>
public partial class MediatorBehaviorAsync : PipelineBehaviorAsync
{
    /// <summary>
    /// Handles the specified request by dispatching it to the appropriate request handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to handle.</typeparam>
    /// <typeparam name="TResponse">The type of the response to return.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns>The response of the handled request.</returns>
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        if (Handlers.TryGet<TRequest>(out var handler))
        {
            if (handler is IRequestHandlerAsync<TRequest, TResponse> requestHandlerAsync)
            {
                return await requestHandlerAsync
                    .Handle(request, cancellationToken)
                    .ConfigureAwait(false);
            }
            else if (handler is IRequestHandler<TRequest, TResponse> requestHandler)
            {
                return requestHandler.Handle(request);
            }
        }

        return await HandleNext<TRequest, TResponse>(request, cancellationToken).ConfigureAwait(false);
    }
}

