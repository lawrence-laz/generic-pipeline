using System;
using GenericPipeline;

namespace ExceptionHandlingExamples;

public class ExceptionHandlingBehavior : PipelineBehavior
{
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        try
        {
            return HandleNext<TRequest, TResponse>(request);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"[!!!] Caught an exception: {exception}");

            return default!;
        }
    }
}

