namespace GenericPipeline.Benchmarks.GenericPipelineAsyncScenario;

public record struct DoWorkRequest() : IRequest<Task<Unit>>;

public class DoWorkHandler : IRequestHandlerAsync<DoWorkRequest>
{
    public async Task<Unit> Handle(DoWorkRequest request)
    {
        await Workload.DoWorkAsync();
        return Unit.Value;
    }
}

public class DoWorkBehavior : PipelineBehaviorAsync
{
    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        await Workload.DoWorkAsync();
        return await HandleNext<TRequest, TResponse>(request);
    }
}

