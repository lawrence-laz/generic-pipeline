namespace GenericPipeline.Benchmarks.GenericPipelineAsyncScenario;

public record struct DoWorkRequest() : IRequest;

public class DoWorkHandler : IRequestHandlerAsync<DoWorkRequest>
{
    public async Task Handle(DoWorkRequest request, CancellationToken cancellationToken)
        => await Workload.DoWorkAsync();
}

public class DoWorkBehavior : PipelineBehaviorAsync
{
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        await Workload.DoWorkAsync();
        return await HandleNext<TRequest, TResponse>(request, cancellationToken);
    }
}

