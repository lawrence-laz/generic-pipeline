namespace GenericPipeline.Benchmarks.PipelineNetAsyncScenario;

// PipelineNet AsyncPipeline requires reference type, so structs cannot be used as requests.
public record DoWorkRequest();

public class DoWorkHandler : PipelineNet.Middleware.IAsyncMiddleware<DoWorkRequest>
{
    public async Task Run(DoWorkRequest parameter, Func<DoWorkRequest, Task> next)
    {
        await Workload.DoWorkAsync();
        await next(parameter);
    }
}

public class DoWorkBehavior : PipelineNet.Middleware.IAsyncMiddleware<DoWorkRequest>
{
    public async Task Run(DoWorkRequest parameter, Func<DoWorkRequest, Task> next)
    {
        await Workload.DoWorkAsync();
        await next(parameter);
    }
}

