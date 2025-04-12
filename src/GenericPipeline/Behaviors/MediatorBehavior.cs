namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that handles requests by dispatching them to the appropriate request handler.
/// </summary>
public partial class MediatorBehavior : PipelineBehavior
{
    /// <summary>
    /// Handles the specified request by dispatching it to the appropriate request handler.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request to handle.</typeparam>
    /// <typeparam name="TResponse">The type of the response to return.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The response of the handled request.</returns>
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        if (Handlers.TryGet<TRequest>(out var handler)
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
