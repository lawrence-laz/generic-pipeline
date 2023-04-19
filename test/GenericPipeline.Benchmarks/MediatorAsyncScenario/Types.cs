using Mediator;

namespace GenericPipeline.Benchmarks.MediatorAsyncScenario;

public record struct DoWorkRequest() : Mediator.IRequest<Mediator.Unit>;

public sealed class DoWorkHandler : Mediator.IRequestHandler<DoWorkRequest>
{
    public async ValueTask<Mediator.Unit> Handle(DoWorkRequest request, CancellationToken cancellationToken)
    {
        await Workload.DoWorkAsync();
        return Mediator.Unit.Value;
    }
}

public class DoWorkBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
    {
        await Workload.DoWorkAsync();
        return await next(message, cancellationToken);
    }
}
