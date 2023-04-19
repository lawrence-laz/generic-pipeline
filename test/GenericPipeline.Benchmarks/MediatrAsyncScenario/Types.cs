using MediatR;

namespace GenericPipeline.Benchmarks.MediatrScenarioAsync;

public record struct DoWorkRequest() : MediatR.IRequest;

/* public class DoWorkHandler : MediatR.IRequestHandler<DoWorkRequest>
{
    public async Task Handle(DoWorkRequest request, CancellationToken cancellationToken)
    {
        await Workload.DoWorkAsync();
    }
} */

public class DoWorkBehavior<TRequest, TResponse> : MediatR.IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        await Workload.DoWorkAsync();
        return await next();
    }
}

