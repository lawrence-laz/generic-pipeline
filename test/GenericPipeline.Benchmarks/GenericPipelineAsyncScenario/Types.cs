namespace GenericPipeline.Benchmarks.GenericPipelineAsyncScenario;

public record struct DoWorkRequest() : IRequest<Unit>;

public class DoWorkHandler : IRequestHandlerAsync<DoWorkRequest>
{
    public async Task<Unit> Handle(DoWorkRequest request, CancellationToken cancellationToken)
    {
        await Workload.DoWorkAsync();
        return Unit.Value;
    }
}

public class DoWorkBehavior : PipelineBehaviorAsync
{
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        await Workload.DoWorkAsync();
        return await HandleNext<TRequest, TResponse>(request, cancellationToken);
    }
}

