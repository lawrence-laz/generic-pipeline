using MediatR;

namespace GenericPipeline.Benchmarks.MediatrScenario;

public record struct DoWorkRequest() : MediatR.IRequest;

public class DoWorkHandler : MediatR.IRequestHandler<DoWorkRequest>
{
    public Task Handle(DoWorkRequest request, CancellationToken cancellationToken)
    {
        Workload.DoWork();
        return Task.CompletedTask;
    }
}

public class DoWorkBehavior<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Workload.DoWork();
        return await next();
    }
}

