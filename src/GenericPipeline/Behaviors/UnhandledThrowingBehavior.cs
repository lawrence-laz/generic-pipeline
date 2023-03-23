namespace GenericPipeline.Behaviors;

/// TODO
public sealed class UnhandledThrowingBehavior : PipelineBehavior
{
    /// TODO
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        // TODO message
        throw new UnhandledRequestException(
            $"Mediator does not accept requests of type " +
            $"'{typeof(TRequest).FullName}' returning '{typeof(TResponse).FullName}'.");
    }
}

