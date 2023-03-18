using System.Reflection;
using BenchmarkDotNet.Running;

// BenchmarkRunner.Run<CompareHandlerCounts>();
BenchmarkRunner.Run<CompareToOtherLibraries>();

public static class BaseBehaviorExtensions
{
    // This provides a MediatR style sending, where response is infered.
    // It is slower however and requires reflection.
    public static TResponse Send<TResponse>(this Pipeline pipeline, GenericPipeline.IRequest<TResponse> request)
    {
        var method = pipeline
            .GetType()
            .GetMethod(nameof(Pipeline.Send), BindingFlags.Public | BindingFlags.Instance)
            ?.MakeGenericMethod(request.GetType(), typeof(TResponse))
            ?? throw new NotSupportedException();

        var response = method.Invoke(pipeline, new[] { request });

        return response is null
            ? default
            : (TResponse)response;
    }
}

