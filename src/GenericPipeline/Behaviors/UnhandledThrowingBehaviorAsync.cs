namespace GenericPipeline.Behaviors;

/// <summary>
/// A pipeline behavior that throws an exception if the request is not handled by any of the handlers in the pipeline.
/// </summary>
public sealed class UnhandledThrowingBehaviorAsync : PipelineBehaviorAsync
{
    /// <summary>
    /// Throws an <see cref="UnhandledRequestException"/> with a message indicating that the request cannot be handled.
    /// </summary>
    /// <typeparam name="TRequest">The type of request that cannot be handled.</typeparam>
    /// <typeparam name="TResponse">The type of response that the request would return.</typeparam>
    /// <param name="request">The request that cannot be handled.</param>
    /// <param name="cancellationToken">The token to cancel the request with.</param>
    /// <returns>This method always throws an exception and does not return a value.</returns>
    /// <exception cref="UnhandledRequestException">Thrown to indicate that the request type is not supported.</exception>
    public override Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        => throw new UnhandledRequestException(
            $"The request of type '{typeof(TRequest).FullName}' returning " +
            $"'{typeof(TResponse).FullName}' was not handled by any handler in the pipeline.");
}
