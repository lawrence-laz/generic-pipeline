namespace GenericPipeline.Internal;

internal static class ServiceProviderExtensions
{
    public static TBehavior GetBehavior<TBehavior>(this IServiceProvider serviceProvider)
        where TBehavior : PipelineBehavior
    {
        if (serviceProvider.GetService(typeof(TBehavior)) is not TBehavior behavior)
        {
            throw new GenericPipelineException(
                $"Failed to resolve behavior '{typeof(TBehavior).Name}' from the service provider.");
        }
        return behavior;
    }

    public static THandler GetHandler<THandler>(this IServiceProvider serviceProvider)
        where THandler : IRequestHandler
    {
        if (serviceProvider.GetService(typeof(THandler)) is not THandler handler)
        {
            throw new GenericPipelineException(
                $"Failed to resolve handler '{typeof(THandler).Name}' from the service provider.");
        }
        return handler;
    }
}

