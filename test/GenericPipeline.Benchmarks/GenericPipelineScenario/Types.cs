namespace GenericPipeline.Benchmarks.GenericPipelineScenario;

public record struct DoWorkRequest() : IRequest;

public class DoWorkHandler : IRequestHandler<DoWorkRequest>
{
    public void Handle(DoWorkRequest request)
    {
        Workload.DoWork();
    }
}

public class DoWorkBehavior : PipelineBehavior
{
    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        Workload.DoWork();
        return HandleNext<TRequest, TResponse>(request);
    }
}

