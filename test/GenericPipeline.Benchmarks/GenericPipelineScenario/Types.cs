namespace GenericPipeline.Benchmarks.GenericPipelineScenario;

public record struct DoWorkRequest() : IRequest;

public class DoWorkHandler : IRequestHandler<DoWorkRequest>
{
    public Unit Handle(DoWorkRequest request)
    {
        Workload.DoWork();
        return Unit.Value;
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
