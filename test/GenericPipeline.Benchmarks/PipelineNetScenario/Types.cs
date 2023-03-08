namespace GenericPipeline.Benchmarks.PipelineNetScenario;

public record struct DoWorkRequest();

public class DoWorkHandler : PipelineNet.Middleware.IMiddleware<DoWorkRequest>
{
    public void Run(DoWorkRequest parameter, Action<DoWorkRequest> next)
    {
        Workload.DoWork();
        next(parameter);
    }
}

public class DoWorkBehavior : PipelineNet.Middleware.IMiddleware<DoWorkRequest>
{
    public void Run(DoWorkRequest parameter, Action<DoWorkRequest> next)
    {
        Workload.DoWork();
        next(parameter);
    }
}

